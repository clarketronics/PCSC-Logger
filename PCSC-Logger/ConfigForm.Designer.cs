namespace PCSC_Logger
{
    partial class ConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.cmbBoxReaders = new System.Windows.Forms.ComboBox();
            this.btnSetFilePath = new System.Windows.Forms.Button();
            this.txtBoxFilePath = new System.Windows.Forms.TextBox();
            this.btnDone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbBoxReaders
            // 
            this.cmbBoxReaders.FormattingEnabled = true;
            this.cmbBoxReaders.Location = new System.Drawing.Point(12, 12);
            this.cmbBoxReaders.Name = "cmbBoxReaders";
            this.cmbBoxReaders.Size = new System.Drawing.Size(297, 28);
            this.cmbBoxReaders.TabIndex = 0;
            this.cmbBoxReaders.SelectedIndexChanged += new System.EventHandler(this.cmbBoxReaders_SelectedIndexChanged);
            this.cmbBoxReaders.Click += new System.EventHandler(this.cmbBoxReaders_Click);
            // 
            // btnSetFilePath
            // 
            this.btnSetFilePath.Location = new System.Drawing.Point(12, 78);
            this.btnSetFilePath.Name = "btnSetFilePath";
            this.btnSetFilePath.Size = new System.Drawing.Size(140, 40);
            this.btnSetFilePath.TabIndex = 1;
            this.btnSetFilePath.Text = "Set file path";
            this.btnSetFilePath.UseVisualStyleBackColor = true;
            this.btnSetFilePath.Click += new System.EventHandler(this.btnSetFilePath_Click);
            // 
            // txtBoxFilePath
            // 
            this.txtBoxFilePath.Location = new System.Drawing.Point(12, 46);
            this.txtBoxFilePath.Name = "txtBoxFilePath";
            this.txtBoxFilePath.ReadOnly = true;
            this.txtBoxFilePath.Size = new System.Drawing.Size(297, 26);
            this.txtBoxFilePath.TabIndex = 2;
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(169, 78);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(140, 40);
            this.btnDone.TabIndex = 3;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 134);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.txtBoxFilePath);
            this.Controls.Add(this.btnSetFilePath);
            this.Controls.Add(this.cmbBoxReaders);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Config";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbBoxReaders;
        private System.Windows.Forms.Button btnSetFilePath;
        private System.Windows.Forms.TextBox txtBoxFilePath;
        private System.Windows.Forms.Button btnDone;
    }
}

