using System;
using System.ServiceProcess;
using System.Windows.Forms;

namespace ServiceMonitor.BuiltinPlugins
{
    public partial class FrmServicePluginConfig : Form
    {
        public readonly ServicePlugin ShadowPlugin;

        public FrmServicePluginConfig(ServicePlugin P)
        {
            if (P is null)
            {
                throw new ArgumentNullException(nameof(P));
            }
            ShadowPlugin = new ServicePlugin();
            InitializeComponent();
            foreach(var Service in ServiceController.GetServices())
            {
                TbServiceName.AutoCompleteCustomSource.Add(Service.ServiceName);
                Service.Dispose();
            }
            foreach(var V in Enum.GetValues(typeof(ServiceControllerStatus)))
            {
                CbStatus.Items.Add(V);
            }
            CbStatus.SelectedItem = P.ExpectedStatus;
            TbServiceName.Text = P.ServiceName;
            TbCheckInterval.Value = P.Interval;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                ShadowPlugin.ServiceName = TbServiceName.Text;
                ShadowPlugin.ExpectedStatus = (ServiceControllerStatus)CbStatus.SelectedItem;
                ShadowPlugin.Interval = (int)TbCheckInterval.Value;
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
