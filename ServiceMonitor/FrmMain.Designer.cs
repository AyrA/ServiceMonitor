
namespace ServiceMonitor
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuTestAll = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuCloseWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuAddPlugin = new System.Windows.Forms.ToolStripMenuItem();
            this.LvPlugins = new System.Windows.Forms.ListView();
            this.ChName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChLastResult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChNextCheck = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Cms = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CmsTestItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CmsConfigItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CmsEnableItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CmsDeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu.SuspendLayout();
            this.Cms.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.MnuAddPlugin});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(584, 24);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuTestAll,
            this.MnuCloseWindow,
            this.MnuExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // MnuTestAll
            // 
            this.MnuTestAll.Name = "MnuTestAll";
            this.MnuTestAll.Size = new System.Drawing.Size(139, 22);
            this.MnuTestAll.Text = "&Test all";
            this.MnuTestAll.Click += new System.EventHandler(this.MnuTestAll_Click);
            // 
            // MnuCloseWindow
            // 
            this.MnuCloseWindow.Name = "MnuCloseWindow";
            this.MnuCloseWindow.Size = new System.Drawing.Size(139, 22);
            this.MnuCloseWindow.Text = "&Close window";
            this.MnuCloseWindow.Click += new System.EventHandler(this.MnuCloseWindow_Click);
            // 
            // MnuExit
            // 
            this.MnuExit.Name = "MnuExit";
            this.MnuExit.Size = new System.Drawing.Size(139, 22);
            this.MnuExit.Text = "&Exit";
            this.MnuExit.Click += new System.EventHandler(this.MnuExit_Click);
            // 
            // MnuAddPlugin
            // 
            this.MnuAddPlugin.Name = "MnuAddPlugin";
            this.MnuAddPlugin.Size = new System.Drawing.Size(70, 20);
            this.MnuAddPlugin.Text = "&Add Plugin";
            // 
            // LvPlugins
            // 
            this.LvPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LvPlugins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ChName,
            this.ChLastResult,
            this.ChNextCheck});
            this.LvPlugins.ContextMenuStrip = this.Cms;
            this.LvPlugins.FullRowSelect = true;
            this.LvPlugins.HideSelection = false;
            this.LvPlugins.Location = new System.Drawing.Point(12, 37);
            this.LvPlugins.MultiSelect = false;
            this.LvPlugins.Name = "LvPlugins";
            this.LvPlugins.Size = new System.Drawing.Size(560, 312);
            this.LvPlugins.TabIndex = 1;
            this.LvPlugins.UseCompatibleStateImageBehavior = false;
            this.LvPlugins.View = System.Windows.Forms.View.Details;
            this.LvPlugins.SelectedIndexChanged += new System.EventHandler(this.LvPlugins_SelectedIndexChanged);
            this.LvPlugins.DoubleClick += new System.EventHandler(this.LvPlugins_DoubleClick);
            this.LvPlugins.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LvPlugins_KeyDown);
            // 
            // ChName
            // 
            this.ChName.Text = "Name";
            this.ChName.Width = 170;
            // 
            // ChLastResult
            // 
            this.ChLastResult.Text = "Last Result";
            this.ChLastResult.Width = 170;
            // 
            // ChNextCheck
            // 
            this.ChNextCheck.Text = "NextCheck";
            this.ChNextCheck.Width = 170;
            // 
            // Cms
            // 
            this.Cms.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmsTestItem,
            this.CmsConfigItem,
            this.CmsEnableItem,
            this.CmsDeleteItem});
            this.Cms.Name = "Cms";
            this.Cms.Size = new System.Drawing.Size(108, 92);
            // 
            // CmsTestItem
            // 
            this.CmsTestItem.Name = "CmsTestItem";
            this.CmsTestItem.Size = new System.Drawing.Size(180, 22);
            this.CmsTestItem.Text = "&Test";
            this.CmsTestItem.Click += new System.EventHandler(this.CmsTestItem_Click);
            // 
            // CmsConfigItem
            // 
            this.CmsConfigItem.Name = "CmsConfigItem";
            this.CmsConfigItem.Size = new System.Drawing.Size(180, 22);
            this.CmsConfigItem.Text = "&Config";
            this.CmsConfigItem.Click += new System.EventHandler(this.CmsConfigItem_Click);
            // 
            // CmsEnableItem
            // 
            this.CmsEnableItem.Name = "CmsEnableItem";
            this.CmsEnableItem.Size = new System.Drawing.Size(180, 22);
            this.CmsEnableItem.Text = "&Enable";
            this.CmsEnableItem.Click += new System.EventHandler(this.CmsEnableItem_Click);
            // 
            // CmsDeleteItem
            // 
            this.CmsDeleteItem.Name = "CmsDeleteItem";
            this.CmsDeleteItem.Size = new System.Drawing.Size(180, 22);
            this.CmsDeleteItem.Text = "&Delete";
            this.CmsDeleteItem.Click += new System.EventHandler(this.CmsDeleteItem_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.LvPlugins);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "FrmMain";
            this.Text = "Service Monitor";
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.Cms.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MnuTestAll;
        private System.Windows.Forms.ToolStripMenuItem MnuCloseWindow;
        private System.Windows.Forms.ToolStripMenuItem MnuExit;
        private System.Windows.Forms.ToolStripMenuItem MnuAddPlugin;
        private System.Windows.Forms.ListView LvPlugins;
        private System.Windows.Forms.ColumnHeader ChName;
        private System.Windows.Forms.ColumnHeader ChLastResult;
        private System.Windows.Forms.ColumnHeader ChNextCheck;
        private System.Windows.Forms.ContextMenuStrip Cms;
        private System.Windows.Forms.ToolStripMenuItem CmsTestItem;
        private System.Windows.Forms.ToolStripMenuItem CmsConfigItem;
        private System.Windows.Forms.ToolStripMenuItem CmsEnableItem;
        private System.Windows.Forms.ToolStripMenuItem CmsDeleteItem;
    }
}

