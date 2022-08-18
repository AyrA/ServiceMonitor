using System;
using System.Windows.Forms;

namespace ServiceMonitor.BuiltinPlugins
{
    public partial class FrmTlsPluginConfig : Form
    {
        public readonly TlsPlugin ShadowPlugin;

        public FrmTlsPluginConfig(TlsPlugin P)
        {
            if (P is null)
            {
                throw new ArgumentNullException(nameof(P));
            }

            InitializeComponent();

            TbConnectionHost.Text = P.ConnectHostName;
            TbConnectionPort.Value = P.ConnectPort;
            TbConnectTimeout.Value = P.Timeout;
            if (!string.IsNullOrEmpty(P.CertificateHostName))
            {
                CbUseCustomName.Checked = true;
                TbCertificateHost.Text = P.CertificateHostName;
            }
            CbChainIgnore.Checked = P.IgnoreChainErrors;
            TbCertificateExpires.Value = P.CertificateLifetimeDays;
            TbCheckInterval.Value = P.Interval / 86400;

            ShadowPlugin = new TlsPlugin();
        }

        private void CbUseCustomName_CheckedChanged(object sender, EventArgs e)
        {
            TbCertificateHost.Enabled = CbUseCustomName.Checked;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            try
            {
                ShadowPlugin.ConnectHostName = TbConnectionHost.Text;
                ShadowPlugin.ConnectPort = (int)TbConnectionPort.Value;
                ShadowPlugin.Timeout = (int)TbConnectTimeout.Value;

                if (CbUseCustomName.Checked && !string.IsNullOrWhiteSpace(TbCertificateHost.Text))
                {
                    ShadowPlugin.CertificateHostName = TbCertificateHost.Text;
                }
                else
                {
                    ShadowPlugin.CertificateHostName = null;
                }
                ShadowPlugin.IgnoreChainErrors = CbChainIgnore.Checked;

                ShadowPlugin.CertificateLifetimeDays = (int)TbCertificateExpires.Value;

                ShadowPlugin.Interval = (int)TbCheckInterval.Value * 86400;

                ShadowPlugin.Validate();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Values invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
