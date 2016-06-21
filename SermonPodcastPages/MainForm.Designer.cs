namespace SermonPodcastPages
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.iTextBoxStatus = new System.Windows.Forms.TextBox();
            this.iButtonOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // iTextBoxStatus
            // 
            this.iTextBoxStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.iTextBoxStatus.Location = new System.Drawing.Point(0, 0);
            this.iTextBoxStatus.Multiline = true;
            this.iTextBoxStatus.Name = "iTextBoxStatus";
            this.iTextBoxStatus.ReadOnly = true;
            this.iTextBoxStatus.Size = new System.Drawing.Size(631, 340);
            this.iTextBoxStatus.TabIndex = 0;
            // 
            // iButtonOK
            // 
            this.iButtonOK.Location = new System.Drawing.Point(257, 356);
            this.iButtonOK.Name = "iButtonOK";
            this.iButtonOK.Size = new System.Drawing.Size(75, 23);
            this.iButtonOK.TabIndex = 1;
            this.iButtonOK.Text = "OK";
            this.iButtonOK.UseVisualStyleBackColor = true;
            this.iButtonOK.Click += new System.EventHandler(this.iButtonOK_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.iButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 391);
            this.Controls.Add(this.iButtonOK);
            this.Controls.Add(this.iTextBoxStatus);
            this.Name = "MainForm";
            this.Text = "Sermon Podcast HTML Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox iTextBoxStatus;
        private System.Windows.Forms.Button iButtonOK;
    }
}