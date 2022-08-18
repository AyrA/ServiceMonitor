
namespace ServiceMonitor.BuiltinPlugins
{
    partial class FrmTlsPluginConfig
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
            this.label1 = new System.Windows.Forms.Label();
            this.TbConnectionHost = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TbConnectionPort = new System.Windows.Forms.NumericUpDown();
            this.CbUseCustomName = new System.Windows.Forms.CheckBox();
            this.TbCertificateHost = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TbCertificateExpires = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.GbConnection = new System.Windows.Forms.GroupBox();
            this.GbSecurity = new System.Windows.Forms.GroupBox();
            this.CbChainIgnore = new System.Windows.Forms.CheckBox();
            this.GbValidation = new System.Windows.Forms.GroupBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.TbConnectTimeout = new System.Windows.Forms.NumericUpDown();
            this.TbCheckInterval = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TbConnectionPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbCertificateExpires)).BeginInit();
            this.GbConnection.SuspendLayout();
            this.GbSecurity.SuspendLayout();
            this.GbValidation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbConnectTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbCheckInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Host:";
            // 
            // TbConnectionHost
            // 
            this.TbConnectionHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbConnectionHost.Location = new System.Drawing.Point(51, 23);
            this.TbConnectionHost.Name = "TbConnectionHost";
            this.TbConnectionHost.Size = new System.Drawing.Size(185, 20);
            this.TbConnectionHost.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port:";
            // 
            // TbConnectionPort
            // 
            this.TbConnectionPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TbConnectionPort.Location = new System.Drawing.Point(291, 24);
            this.TbConnectionPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.TbConnectionPort.Name = "TbConnectionPort";
            this.TbConnectionPort.Size = new System.Drawing.Size(69, 20);
            this.TbConnectionPort.TabIndex = 3;
            // 
            // CbUseCustomName
            // 
            this.CbUseCustomName.AutoSize = true;
            this.CbUseCustomName.Location = new System.Drawing.Point(11, 21);
            this.CbUseCustomName.Name = "CbUseCustomName";
            this.CbUseCustomName.Size = new System.Drawing.Size(157, 17);
            this.CbUseCustomName.TabIndex = 0;
            this.CbUseCustomName.Text = "Custom name on certificate:";
            this.CbUseCustomName.UseVisualStyleBackColor = true;
            this.CbUseCustomName.CheckedChanged += new System.EventHandler(this.CbUseCustomName_CheckedChanged);
            // 
            // TbCertificateHost
            // 
            this.TbCertificateHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbCertificateHost.Location = new System.Drawing.Point(174, 19);
            this.TbCertificateHost.Name = "TbCertificateHost";
            this.TbCertificateHost.Size = new System.Drawing.Size(186, 20);
            this.TbCertificateHost.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Alert if expiration within";
            // 
            // TbCertificateExpires
            // 
            this.TbCertificateExpires.Location = new System.Drawing.Point(130, 22);
            this.TbCertificateExpires.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.TbCertificateExpires.Name = "TbCertificateExpires";
            this.TbCertificateExpires.Size = new System.Drawing.Size(69, 20);
            this.TbCertificateExpires.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(205, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "days";
            // 
            // GbConnection
            // 
            this.GbConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GbConnection.Controls.Add(this.TbConnectTimeout);
            this.GbConnection.Controls.Add(this.TbConnectionHost);
            this.GbConnection.Controls.Add(this.label5);
            this.GbConnection.Controls.Add(this.label1);
            this.GbConnection.Controls.Add(this.label2);
            this.GbConnection.Controls.Add(this.label7);
            this.GbConnection.Controls.Add(this.TbConnectionPort);
            this.GbConnection.Location = new System.Drawing.Point(12, 12);
            this.GbConnection.Name = "GbConnection";
            this.GbConnection.Size = new System.Drawing.Size(378, 87);
            this.GbConnection.TabIndex = 0;
            this.GbConnection.TabStop = false;
            this.GbConnection.Text = "Connection";
            // 
            // GbSecurity
            // 
            this.GbSecurity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GbSecurity.Controls.Add(this.CbChainIgnore);
            this.GbSecurity.Controls.Add(this.TbCertificateHost);
            this.GbSecurity.Controls.Add(this.CbUseCustomName);
            this.GbSecurity.Location = new System.Drawing.Point(12, 105);
            this.GbSecurity.Name = "GbSecurity";
            this.GbSecurity.Size = new System.Drawing.Size(378, 82);
            this.GbSecurity.TabIndex = 1;
            this.GbSecurity.TabStop = false;
            this.GbSecurity.Text = "Security overrides";
            // 
            // CbChainIgnore
            // 
            this.CbChainIgnore.AutoSize = true;
            this.CbChainIgnore.Location = new System.Drawing.Point(11, 48);
            this.CbChainIgnore.Name = "CbChainIgnore";
            this.CbChainIgnore.Size = new System.Drawing.Size(266, 17);
            this.CbChainIgnore.TabIndex = 2;
            this.CbChainIgnore.Text = "Ignore chain errors and self signed certificate errors";
            this.CbChainIgnore.UseVisualStyleBackColor = true;
            // 
            // GbValidation
            // 
            this.GbValidation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GbValidation.Controls.Add(this.TbCertificateExpires);
            this.GbValidation.Controls.Add(this.label3);
            this.GbValidation.Controls.Add(this.label4);
            this.GbValidation.Location = new System.Drawing.Point(12, 193);
            this.GbValidation.Name = "GbValidation";
            this.GbValidation.Size = new System.Drawing.Size(378, 60);
            this.GbValidation.TabIndex = 2;
            this.GbValidation.TabStop = false;
            this.GbValidation.Text = "Validation checks";
            // 
            // BtnOk
            // 
            this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOk.Location = new System.Drawing.Point(234, 299);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 6;
            this.BtnOk.Text = "&Ok";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(315, 299);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 7;
            this.BtnCancel.Text = "&Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // TbConnectTimeout
            // 
            this.TbConnectTimeout.Location = new System.Drawing.Point(130, 54);
            this.TbConnectTimeout.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.TbConnectTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TbConnectTimeout.Name = "TbConnectTimeout";
            this.TbConnectTimeout.Size = new System.Drawing.Size(120, 20);
            this.TbConnectTimeout.TabIndex = 5;
            this.TbConnectTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // TbCheckInterval
            // 
            this.TbCheckInterval.Location = new System.Drawing.Point(107, 263);
            this.TbCheckInterval.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.TbCheckInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TbCheckInterval.Name = "TbCheckInterval";
            this.TbCheckInterval.Size = new System.Drawing.Size(73, 20);
            this.TbCheckInterval.TabIndex = 4;
            this.TbCheckInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(258, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Value in milliseconds";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(186, 265);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Value in days";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Connection timeout";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 265);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Check interval";
            // 
            // FrmTlsPluginConfig
            // 
            this.AcceptButton = this.BtnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(402, 334);
            this.Controls.Add(this.TbCheckInterval);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.GbValidation);
            this.Controls.Add(this.GbSecurity);
            this.Controls.Add(this.GbConnection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTlsPluginConfig";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TLS Check Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.TbConnectionPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbCertificateExpires)).EndInit();
            this.GbConnection.ResumeLayout(false);
            this.GbConnection.PerformLayout();
            this.GbSecurity.ResumeLayout(false);
            this.GbSecurity.PerformLayout();
            this.GbValidation.ResumeLayout(false);
            this.GbValidation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbConnectTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbCheckInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TbConnectionHost;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown TbConnectionPort;
        private System.Windows.Forms.CheckBox CbUseCustomName;
        private System.Windows.Forms.TextBox TbCertificateHost;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown TbCertificateExpires;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox GbConnection;
        private System.Windows.Forms.GroupBox GbSecurity;
        private System.Windows.Forms.CheckBox CbChainIgnore;
        private System.Windows.Forms.GroupBox GbValidation;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.NumericUpDown TbConnectTimeout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown TbCheckInterval;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
    }
}