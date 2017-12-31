namespace OrbisTitleMetadataDatabase
{
    partial class frmExtra
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExtra));
            this.tcExtra = new System.Windows.Forms.TabControl();
            this.tpBackgroundImage = new System.Windows.Forms.TabPage();
            this.picBackgroundImage = new System.Windows.Forms.PictureBox();
            this.tpBackgroundMusic = new System.Windows.Forms.TabPage();
            this.axWMP = new AxWMPLib.AxWindowsMediaPlayer();
            this.tpAdvanced = new System.Windows.Forms.TabPage();
            this.txtAdvanced = new System.Windows.Forms.TextBox();
            this.pnlPronunciation = new System.Windows.Forms.Panel();
            this.txtLanguage = new System.Windows.Forms.TextBox();
            this.lblLanguageId = new System.Windows.Forms.Label();
            this.cboLanguageId = new System.Windows.Forms.ComboBox();
            this.tpPronunciation = new System.Windows.Forms.TabPage();
            this.cboPronunciation = new System.Windows.Forms.ComboBox();
            this.tcExtra.SuspendLayout();
            this.tpBackgroundImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBackgroundImage)).BeginInit();
            this.tpBackgroundMusic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWMP)).BeginInit();
            this.tpAdvanced.SuspendLayout();
            this.pnlPronunciation.SuspendLayout();
            this.tpPronunciation.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcExtra
            // 
            this.tcExtra.Controls.Add(this.tpBackgroundImage);
            this.tcExtra.Controls.Add(this.tpBackgroundMusic);
            this.tcExtra.Controls.Add(this.tpPronunciation);
            this.tcExtra.Controls.Add(this.tpAdvanced);
            this.tcExtra.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcExtra.Location = new System.Drawing.Point(0, 0);
            this.tcExtra.Name = "tcExtra";
            this.tcExtra.SelectedIndex = 0;
            this.tcExtra.Size = new System.Drawing.Size(484, 261);
            this.tcExtra.TabIndex = 0;
            this.tcExtra.TabStop = false;
            this.tcExtra.SelectedIndexChanged += new System.EventHandler(this.tcExtra_SelectedIndexChanged);
            // 
            // tpBackgroundImage
            // 
            this.tpBackgroundImage.Controls.Add(this.picBackgroundImage);
            this.tpBackgroundImage.Location = new System.Drawing.Point(4, 22);
            this.tpBackgroundImage.Name = "tpBackgroundImage";
            this.tpBackgroundImage.Padding = new System.Windows.Forms.Padding(3);
            this.tpBackgroundImage.Size = new System.Drawing.Size(476, 235);
            this.tpBackgroundImage.TabIndex = 0;
            this.tpBackgroundImage.Text = "Background Image";
            this.tpBackgroundImage.UseVisualStyleBackColor = true;
            // 
            // picBackgroundImage
            // 
            this.picBackgroundImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBackgroundImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBackgroundImage.Location = new System.Drawing.Point(3, 3);
            this.picBackgroundImage.Name = "picBackgroundImage";
            this.picBackgroundImage.Size = new System.Drawing.Size(470, 229);
            this.picBackgroundImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBackgroundImage.TabIndex = 0;
            this.picBackgroundImage.TabStop = false;
            this.picBackgroundImage.DoubleClick += new System.EventHandler(this.picBackgroundImage_DoubleClick);
            this.picBackgroundImage.MouseHover += new System.EventHandler(this.picBackgroundImage_MouseHover);
            // 
            // tpBackgroundMusic
            // 
            this.tpBackgroundMusic.Controls.Add(this.axWMP);
            this.tpBackgroundMusic.Location = new System.Drawing.Point(4, 22);
            this.tpBackgroundMusic.Name = "tpBackgroundMusic";
            this.tpBackgroundMusic.Padding = new System.Windows.Forms.Padding(3);
            this.tpBackgroundMusic.Size = new System.Drawing.Size(476, 235);
            this.tpBackgroundMusic.TabIndex = 3;
            this.tpBackgroundMusic.Text = "Background Music";
            this.tpBackgroundMusic.UseVisualStyleBackColor = true;
            // 
            // axWMP
            // 
            this.axWMP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axWMP.Enabled = true;
            this.axWMP.Location = new System.Drawing.Point(3, 3);
            this.axWMP.Name = "axWMP";
            this.axWMP.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWMP.OcxState")));
            this.axWMP.Size = new System.Drawing.Size(470, 229);
            this.axWMP.TabIndex = 0;
            // 
            // tpAdvanced
            // 
            this.tpAdvanced.Controls.Add(this.txtAdvanced);
            this.tpAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tpAdvanced.Name = "tpAdvanced";
            this.tpAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tpAdvanced.Size = new System.Drawing.Size(476, 235);
            this.tpAdvanced.TabIndex = 2;
            this.tpAdvanced.Text = "Advanced";
            this.tpAdvanced.UseVisualStyleBackColor = true;
            // 
            // txtAdvanced
            // 
            this.txtAdvanced.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAdvanced.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdvanced.Location = new System.Drawing.Point(3, 3);
            this.txtAdvanced.Multiline = true;
            this.txtAdvanced.Name = "txtAdvanced";
            this.txtAdvanced.ReadOnly = true;
            this.txtAdvanced.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAdvanced.Size = new System.Drawing.Size(470, 229);
            this.txtAdvanced.TabIndex = 0;
            this.txtAdvanced.WordWrap = false;
            // 
            // pnlPronunciation
            // 
            this.pnlPronunciation.Controls.Add(this.txtLanguage);
            this.pnlPronunciation.Location = new System.Drawing.Point(8, 33);
            this.pnlPronunciation.Name = "pnlPronunciation";
            this.pnlPronunciation.Size = new System.Drawing.Size(460, 196);
            this.pnlPronunciation.TabIndex = 4;
            // 
            // txtLanguage
            // 
            this.txtLanguage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLanguage.Location = new System.Drawing.Point(0, 0);
            this.txtLanguage.Multiline = true;
            this.txtLanguage.Name = "txtLanguage";
            this.txtLanguage.ReadOnly = true;
            this.txtLanguage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLanguage.Size = new System.Drawing.Size(460, 196);
            this.txtLanguage.TabIndex = 0;
            this.txtLanguage.WordWrap = false;
            // 
            // lblLanguageId
            // 
            this.lblLanguageId.AutoSize = true;
            this.lblLanguageId.Location = new System.Drawing.Point(6, 9);
            this.lblLanguageId.Name = "lblLanguageId";
            this.lblLanguageId.Size = new System.Drawing.Size(70, 13);
            this.lblLanguageId.TabIndex = 5;
            this.lblLanguageId.Text = "Language Id:";
            // 
            // cboLanguageId
            // 
            this.cboLanguageId.Enabled = false;
            this.cboLanguageId.FormattingEnabled = true;
            this.cboLanguageId.Location = new System.Drawing.Point(82, 6);
            this.cboLanguageId.Name = "cboLanguageId";
            this.cboLanguageId.Size = new System.Drawing.Size(40, 21);
            this.cboLanguageId.TabIndex = 6;
            this.cboLanguageId.SelectedIndexChanged += new System.EventHandler(this.cboLanguageId_SelectedIndexChanged);
            // 
            // tpPronunciation
            // 
            this.tpPronunciation.Controls.Add(this.cboPronunciation);
            this.tpPronunciation.Controls.Add(this.cboLanguageId);
            this.tpPronunciation.Controls.Add(this.lblLanguageId);
            this.tpPronunciation.Controls.Add(this.pnlPronunciation);
            this.tpPronunciation.Location = new System.Drawing.Point(4, 22);
            this.tpPronunciation.Name = "tpPronunciation";
            this.tpPronunciation.Padding = new System.Windows.Forms.Padding(3);
            this.tpPronunciation.Size = new System.Drawing.Size(476, 235);
            this.tpPronunciation.TabIndex = 1;
            this.tpPronunciation.Text = "Pronunciation";
            this.tpPronunciation.UseVisualStyleBackColor = true;
            // 
            // cboPronunciation
            // 
            this.cboPronunciation.Enabled = false;
            this.cboPronunciation.FormattingEnabled = true;
            this.cboPronunciation.Location = new System.Drawing.Point(128, 6);
            this.cboPronunciation.Name = "cboPronunciation";
            this.cboPronunciation.Size = new System.Drawing.Size(115, 21);
            this.cboPronunciation.TabIndex = 7;
            this.cboPronunciation.SelectedIndexChanged += new System.EventHandler(this.cboPronunciation_SelectedIndexChanged);
            // 
            // frmExtra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.tcExtra);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmExtra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmExtra";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmExtra_FormClosing);
            this.tcExtra.ResumeLayout(false);
            this.tpBackgroundImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBackgroundImage)).EndInit();
            this.tpBackgroundMusic.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWMP)).EndInit();
            this.tpAdvanced.ResumeLayout(false);
            this.tpAdvanced.PerformLayout();
            this.pnlPronunciation.ResumeLayout(false);
            this.pnlPronunciation.PerformLayout();
            this.tpPronunciation.ResumeLayout(false);
            this.tpPronunciation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcExtra;
        private System.Windows.Forms.TabPage tpAdvanced;
        private System.Windows.Forms.TabPage tpBackgroundImage;
        private System.Windows.Forms.PictureBox picBackgroundImage;
        private System.Windows.Forms.TextBox txtAdvanced;
        private System.Windows.Forms.TabPage tpBackgroundMusic;
        private AxWMPLib.AxWindowsMediaPlayer axWMP;
        private System.Windows.Forms.TabPage tpPronunciation;
        private System.Windows.Forms.ComboBox cboPronunciation;
        private System.Windows.Forms.ComboBox cboLanguageId;
        private System.Windows.Forms.Label lblLanguageId;
        private System.Windows.Forms.Panel pnlPronunciation;
        private System.Windows.Forms.TextBox txtLanguage;
    }
}