using System;
using System.Windows.Forms;

namespace ServiceMonitor.BuiltinPlugins
{
    public partial class FrmIcmpPluginConfig : Form
    {
        public readonly IcmpPlugin ShadowPlugin;

        public FrmIcmpPluginConfig(IcmpPlugin P)
        {
            if (P is null)
            {
                throw new ArgumentNullException(nameof(P));
            }

            InitializeComponent();
            TbHostname.Text = P.HostName;
            TbCheckInterval.Value = P.Interval;
            TbTimeout.Value = P.Timeout;
            ShadowPlugin = new IcmpPlugin();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                ShadowPlugin.HostName = TbHostname.Text;
                ShadowPlugin.Interval = (int)TbCheckInterval.Value;
                ShadowPlugin.Timeout = (int)TbTimeout.Value;
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
