using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace ServiceMonitor
{
    public class SafeMemoryHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        public SafeMemoryHandle(int Size) : base(IntPtr.Zero, true)
        {
            if (Size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(Size));
            }
            SetHandle(Marshal.AllocHGlobal(Size));
        }

        public SafeMemoryHandle(IntPtr ExistingHandle) : base(IntPtr.Zero, true)
        {
            SetHandle(ExistingHandle);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            Marshal.FreeHGlobal(handle);
            return true;
        }
    }
}
