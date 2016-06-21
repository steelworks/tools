namespace ImageProcessor2
{
    partial class FormMain
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
            this.iImageText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // iImageText
            // 
            this.iImageText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iImageText.Location = new System.Drawing.Point(0, 0);
            this.iImageText.Multiline = true;
            this.iImageText.Name = "iImageText";
            this.iImageText.ReadOnly = true;
            this.iImageText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.iImageText.Size = new System.Drawing.Size(284, 262);
            this.iImageText.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.iImageText);
            this.Name = "FormMain";
            this.Text = "Image Processor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox iImageText;
    }
}

