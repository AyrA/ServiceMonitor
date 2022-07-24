using System;
using System.Windows.Forms;

namespace ServiceMonitor.BuiltinPlugins
{
    public partial class FrmHttpPluginConfig : Form
    {
        public readonly HttpPlugin ShadowPlugin;

        public FrmHttpPluginConfig(HttpPlugin P)
        {
            if (P is null)
            {
                throw new ArgumentNullException(nameof(P));
            }

            InitializeComponent();
            TbURL.Text = P.URL.ToString();
            TbCheckInterval.Value = P.Interval;
            TbConnectTimeout.Value = P.TcpTimeout;
            TbStatusCode.Value = P.StatusCode;
            CbIgnoreSslErrors.Checked = P.IgnoreTlsError;
            ShadowPlugin = new HttpPlugin();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                ShadowPlugin.URL = new Uri(TbURL.Text);
                ShadowPlugin.Interval = (int)TbCheckInterval.Value;
                ShadowPlugin.TcpTimeout = (int)TbConnectTimeout.Value;
                ShadowPlugin.StatusCode = (int)TbStatusCode.Value;
                ShadowPlugin.IgnoreTlsError = CbIgnoreSslErrors.Checked;
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
