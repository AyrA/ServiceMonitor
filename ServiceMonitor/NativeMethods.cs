using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ServiceMonitor
{
    public static class NativeMethods
    {
        [DllImport("Crypt32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptProtectData(
            ref DATA_BLOB pDataIn,
            string szDataDescr,
            //ref DATA_BLOB pOptionalEntropy,
            IntPtr pOptionalEntropy,
            IntPtr pvReserved,
            ref CRYPTPROTECT_PROMPTSTRUCT pPromptStruct,
            CryptProtectFlags dwFlags,
            ref DATA_BLOB pDataOut
        );

        [DllImport("Crypt32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptUnprotectData(
            ref DATA_BLOB pDataIn,
            string szDataDescr,
            //ref DATA_BLOB pOptionalEntropy,
            IntPtr pOptionalEntropy,
            IntPtr pvReserved,
            ref CRYPTPROTECT_PROMPTSTRUCT pPromptStruct,
            CryptProtectFlags dwFlags,
            ref DATA_BLOB pDataOut
        );


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct DATA_BLOB
        {
            public int Length;
            public IntPtr Data;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct CRYPTPROTECT_PROMPTSTRUCT
        {
            public int cbSize;
            public CryptProtectPromptFlags dwPromptFlags;
            public IntPtr hwndApp;
            public string szPrompt;
        }

        [Flags]
        private enum CryptProtectFlags
        {
            /// <summary>
            /// for remote-access situations where ui is not an option
            /// if UI was specified on protect or unprotect operation, the call
            /// will fail and GetLastError() will indicate ERROR_PASSWORD_RESTRICTION
            /// </summary>
            CRYPTPROTECT_UI_FORBIDDEN = 0x1,

            /// <summary>
            /// per machine protected data -- any user on machine where CryptProtectData
            /// took place may CryptUnprotectData
            /// </summary>
            CRYPTPROTECT_LOCAL_MACHINE = 0x4,

            /// <summary>
            /// force credential synchronize during CryptProtectData()
            /// Synchronize is only operation that occurs during this operation
            /// </summary>
            CRYPTPROTECT_CRED_SYNC = 0x8,

            /// <summary>
            /// Generate an Audit on protect and unprotect operations
            /// </summary>
            CRYPTPROTECT_AUDIT = 0x10,

            /// <summary>
            /// Protect data with a non-recoverable key
            /// </summary>
            CRYPTPROTECT_NO_RECOVERY = 0x20,

            /// <summary>
            /// Verify the protection of a protected blob
            /// </summary>
            CRYPTPROTECT_VERIFY_PROTECTION = 0x40
        }

        [Flags]
        private enum CryptProtectPromptFlags
        {
            /// <summary>
            /// prompt on unprotect
            /// </summary>
            CRYPTPROTECT_PROMPT_ON_UNPROTECT = 0x1,

            /// <summary>
            /// prompt on protect
            /// </summary>
            CRYPTPROTECT_PROMPT_ON_PROTECT = 0x2
        }

        public static byte[] Encrypt(byte[] Data)
        {
            if (Data is null)
            {
                throw new ArgumentNullException(nameof(Data));
            }

            using (var PtrIn = new SafeMemoryHandle(Data.Length))
            {
                var SourceData = new DATA_BLOB()
                {
                    Length = Data.Length,
                    Data = PtrIn.DangerousGetHandle()
                };
                var OutData = new DATA_BLOB();
                var Prompt = new CRYPTPROTECT_PROMPTSTRUCT()
                {
                    cbSize = 12,
                    dwPromptFlags = 0,
                    hwndApp = IntPtr.Zero
                };
                Marshal.Copy(Data, 0, SourceData.Data, Data.Length);

                if (!CryptProtectData(ref SourceData, null, IntPtr.Zero, IntPtr.Zero, ref Prompt, 0, ref OutData))
                {
                    throw new Win32Exception();
                }

                using (var PtrOut = new SafeMemoryHandle(OutData.Data))
                {
                    var Out = new byte[OutData.Length];
                    Marshal.Copy(OutData.Data, Out, 0, Out.Length);
                    return Out;
                }
            }
        }

        public static byte[] Decrypt(byte[] Data)
        {
            if (Data is null)
            {
                throw new ArgumentNullException(nameof(Data));
            }

            using (var PtrIn = new SafeMemoryHandle(Data.Length))
            {
                var SourceData = new DATA_BLOB()
                {
                    Length = Data.Length,
                    Data = PtrIn.DangerousGetHandle()
                };
                var OutData = new DATA_BLOB();
                var Prompt = new CRYPTPROTECT_PROMPTSTRUCT()
                {
                    cbSize = 12,
                    dwPromptFlags = 0,
                    hwndApp = IntPtr.Zero
                };
                Marshal.Copy(Data, 0, SourceData.Data, Data.Length);

                if (!CryptUnprotectData(ref SourceData, null, IntPtr.Zero, IntPtr.Zero, ref Prompt, 0, ref OutData))
                {
                    throw new Win32Exception();
                }
                using (var PtrOut = new SafeMemoryHandle(OutData.Data))
                {
                    var Out = new byte[OutData.Length];
                    Marshal.Copy(OutData.Data, Out, 0, Out.Length);
                    return Out;
                }
            }
        }
    }
}
