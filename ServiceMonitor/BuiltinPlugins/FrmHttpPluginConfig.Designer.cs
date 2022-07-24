
namespace ServiceMonitor.BuiltinPlugins
{
    partial class FrmHttpPluginConfig
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
            this.TbURL = new System.Windows.Forms.TextBox();
            this.LblURL = new System.Windows.Forms.Label();
            this.CbIgnoreSslErrors = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TbStatusCode = new System.Windows.Forms.NumericUpDown();
            this.TbCheckInterval = new System.Windows.Forms.NumericUpDown();
            this.TbConnectTimeout = new System.Windows.Forms.NumericUpDown();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TbStatusCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbCheckInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbConnectTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // TbURL
            // 
            this.TbURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbURL.Location = new System.Drawing.Point(47, 12);
            this.TbURL.Name = "TbURL";
            this.TbURL.Size = new System.Drawing.Size(325, 20);
            this.TbURL.TabIndex = 1;
            // 
            // LblURL
            // 
            this.LblURL.AutoSize = true;
            this.LblURL.Location = new System.Drawing.Point(12, 15);
            this.LblURL.Name = "LblURL";
            this.LblURL.Size = new System.Drawing.Size(29, 13);
            this.LblURL.TabIndex = 0;
            this.LblURL.Text = "URL";
            // 
            // CbIgnoreSslErrors
            // 
            this.CbIgnoreSslErrors.AutoSize = true;
            this.CbIgnoreSslErrors.Location = new System.Drawing.Point(12, 43);
            this.CbIgnoreSslErrors.Name = "CbIgnoreSslErrors";
            this.CbIgnoreSslErrors.Size = new System.Drawing.Size(108, 17);
            this.CbIgnoreSslErrors.TabIndex = 2;
            this.CbIgnoreSslErrors.Text = "Ignore SSL errors";
            this.CbIgnoreSslErrors.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Check interval";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Connection timeout";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Expected status code";
            // 
            // TbStatusCode
            // 
            this.TbStatusCode.Location = new System.Drawing.Point(128, 71);
            this.TbStatusCode.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.TbStatusCode.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TbStatusCode.Name = "TbStatusCode";
            this.TbStatusCode.Size = new System.Drawing.Size(120, 20);
            this.TbStatusCode.TabIndex = 4;
            this.TbStatusCode.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // TbCheckInterval
            // 
            this.TbCheckInterval.Location = new System.Drawing.Point(128, 102);
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
            this.TbCheckInterval.Size = new System.Drawing.Size(120, 20);
            this.TbCheckInterval.TabIndex = 6;
            this.TbCheckInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // TbConnectTimeout
            // 
            this.TbConnectTimeout.Location = new System.Drawing.Point(128, 133);
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
            this.TbConnectTimeout.TabIndex = 9;
            this.TbConnectTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOK.Location = new System.Drawing.Point(216, 166);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 11;
            this.BtnOK.Text = "&OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(297, 166);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 12;
            this.BtnCancel.Text = "&Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(256, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Value in seconds";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(256, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Value in milliseconds";
            // 
            // FrmHttpPluginConfig
            // 
            this.AcceptButton = this.BtnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(384, 201);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.TbConnectTimeout);
            this.Controls.Add(this.TbCheckInterval);
            this.Controls.Add(this.TbStatusCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CbIgnoreSslErrors);
            this.Controls.Add(this.LblURL);
            this.Controls.Add(this.TbURL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmHttpPluginConfig";
            this.ShowInTaskbar = false;
            this.Text = "HTTP Configuration";
            ((System.ComponentModel.ISupportInitialize)(this.TbStatusCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbCheckInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbConnectTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TbURL;
        private System.Windows.Forms.Label LblURL;
        private System.Windows.Forms.CheckBox CbIgnoreSslErrors;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown TbStatusCode;
        private System.Windows.Forms.NumericUpDown TbCheckInterval;
        private System.Windows.Forms.NumericUpDown TbConnectTimeout;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}