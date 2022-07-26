using ServiceMonitor.Properties;
using System;
using System.Linq;
using System.Windows.Forms;

namespace ServiceMonitor
{
    public partial class FrmMain : Form
    {
        private const int LED_OFF = 0;
        private const int LED_OK = LED_OFF + 1;
        private const int LED_WARN = LED_OK + 1;
        private const int LED_ERR = LED_WARN + 1;

        public FrmMain()
        {
            InitializeComponent();
            LvPlugins.SmallImageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new System.Drawing.Size(16, 16)
            };
            LvPlugins.SmallImageList.Images.Add(Resources.LedCheckOff);
            LvPlugins.SmallImageList.Images.Add(Resources.LedCheckOk);
            LvPlugins.SmallImageList.Images.Add(Resources.LedCheckWarning);
            LvPlugins.SmallImageList.Images.Add(Resources.LedCheckError);
            ShowPlugins();
        }

        public void SetMenuPlugins(PluginInfo[] infos)
        {
            MnuAddPlugin.DropDownItems.Clear();
            foreach (var info in infos)
            {
                var item = MnuAddPlugin.DropDownItems.Add(info.BaseName);
                item.Tag = info;
                item.Click += AddTest;
            }
        }

        public void ShowPlugins()
        {
            LvPlugins.SuspendLayout();
            ClearEvents();
            LvPlugins.Items.Clear();
            foreach (var PI in PluginManager.Plugins.OrderBy(m => m.Plugin.PluginType.FullName).ThenBy(m => m.Plugin.Name))
            {
                var Entry = LvPlugins.Items.Add(PI.Plugin.Name);
                Entry.Tag = PI;
                Entry.SubItems.Add(string.Empty);
                Entry.SubItems.Add(string.Empty);
                UpdateListItem(Entry, PI);
                PI.TestComplete += PluginTestComplete;
            }
            LvPlugins.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            LvPlugins.ResumeLayout();
        }

        private void AddTest(object sender, EventArgs e)
        {
            if (!(sender is ToolStripItem Item))
            {
                throw new ArgumentException($"Expected {nameof(ToolStripItem)} but got {sender.GetType().FullName}");
            }
            var PI = (PluginInfo)Item.Tag;
            var P = PluginManager.LoadPlugin(PI);
            if (P.Config())
            {
                PluginManager.Plugins.Add(new PluginStatus(P));
                P.Start();
                PluginManager.SavePluginData();
                ShowPlugins();
            }
            else
            {
                P.Dispose();
            }
        }

        private void ClearEvents()
        {
            foreach (var Item in LvPlugins.Items.OfType<ListViewItem>())
            {
                ((PluginStatus)Item.Tag).TestComplete -= PluginTestComplete;
            }
        }

        private void UpdateListItem(ListViewItem item, PluginStatus info)
        {
            item.Text = info.Plugin.Name;
            if (info.Enabled)
            {
                item.SubItems[1].Text = info.LastError == null ? info.Plugin.LastStatus : info.LastError.Message;
                item.SubItems[2].Text = info.Plugin.NextCheck.ToLocalTime().ToString("G");
            }
            else
            {
                item.SubItems[1].Text = "Disabled";
                item.SubItems[2].Text = "<None>";
            }
            if (!info.Enabled)
            {
                item.ImageIndex = LED_OFF;
            }
            else
            {
                if (info.IsTesting)
                {
                    item.ImageIndex = LED_WARN;
                }
                else
                {
                    item.ImageIndex = info.LastError == null ? LED_OK : LED_ERR;
                }
            }
        }

        private void AskDelete(ListViewItem item)
        {
            var info = (PluginStatus)item.Tag;
            if (MessageBox.Show("Delete this plugin?", "Delete " + info.Plugin.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                item.Remove();
                PluginManager.Plugins.Remove(info);
                info.TestComplete -= PluginTestComplete;
                PluginManager.SavePluginData();
            }
        }

        #region Event handler

        private void MnuTestAll_Click(object sender, EventArgs e)
        {
            foreach (var PI in PluginManager.Plugins)
            {
                try
                {
                    PI.BeginTest(false);
                }
                catch
                {
                    //NOOP
                }
            }
        }

        private void MnuCloseWindow_Click(object sender, EventArgs e)
        {
            ClearEvents();
            Close();
        }

        private void MnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PluginTestComplete(PluginStatus sender, Exception previousResult)
        {
            //Run event results in UI thread
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    PluginTestComplete(sender, previousResult);
                });
                return;
            }
            var ErrorChange = false;
            if ((sender.LastError == null && previousResult != null) ||
                (sender.LastError != null && previousResult == null))
            {
                ErrorChange = true;
            }
            if (ErrorChange)
            {
                if (sender.LastError == null)
                {
                    Program.ShowInfo("Error condition resolved", sender.Plugin.Name, ToolTipIcon.Info);
                }
                else
                {
                    Program.ShowInfo($"Service status alert:\r\n{sender.LastError.Message}", sender.Plugin.Name, ToolTipIcon.Error);
                }
            }
            var ListItem = LvPlugins.Items.OfType<ListViewItem>().FirstOrDefault(m => ((PluginStatus)m.Tag) == sender);
            if (ListItem != null)
            {
                UpdateListItem(ListItem, sender);
                LvPlugins.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
            else
            {
                sender.TestComplete -= PluginTestComplete;
            }
        }

        private void LvPlugins_DoubleClick(object sender, EventArgs e)
        {
            if (LvPlugins.SelectedItems.Count > 0)
            {
                var item = LvPlugins.SelectedItems[0];
                var info = (PluginStatus)item.Tag;
                if (info.Plugin.Config())
                {
                    PluginManager.SavePluginData();
                    UpdateListItem(item, info);
                }
            }
        }

        private void LvPlugins_KeyDown(object sender, KeyEventArgs e)
        {
            if (LvPlugins.SelectedItems.Count == 0)
            {
                return;
            }
            var item = LvPlugins.SelectedItems[0];
            var info = (PluginStatus)item.Tag;
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (info.Plugin.Config())
                    {
                        PluginManager.SavePluginData();
                        UpdateListItem(item, info);
                    }
                    break;
                case Keys.Delete:
                    AskDelete(item);
                    break;
            }
        }

        private void CmsTestItem_Click(object sender, EventArgs e)
        {
            var Item = LvPlugins.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            if (Item != null)
            {
                var Plugin = (PluginStatus)Item.Tag;
                Plugin.BeginTest(false);
            }
        }

        private void CmsConfigItem_Click(object sender, EventArgs e)
        {
            var Item = LvPlugins.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            if (Item != null)
            {
                var Plugin = (PluginStatus)Item.Tag;
                if (Plugin.Plugin.Config())
                {
                    PluginManager.SavePluginData();
                    UpdateListItem(Item, Plugin);
                }
            }
        }

        private void CmsEnableItem_Click(object sender, EventArgs e)
        {
            var Item = LvPlugins.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            if (Item != null)
            {
                var Plugin = (PluginStatus)Item.Tag;
                Plugin.Enabled = !Plugin.Enabled;
                CmsEnableItem.Checked = !CmsEnableItem.Checked;
                UpdateListItem(Item, Plugin);
                PluginManager.SavePluginData();
            }
        }

        private void CmsDeleteItem_Click(object sender, EventArgs e)
        {
            var Item = LvPlugins.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            if (Item != null)
            {
                AskDelete(Item);
            }
        }

        private void LvPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            var Item = LvPlugins.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
            if (Item != null)
            {
                CmsEnableItem.Checked = ((PluginStatus)Item.Tag).Enabled;
            }
        }

        #endregion
    }
}
