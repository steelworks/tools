namespace Speak
{
    partial class Speak
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
            this.speechBox = new System.Windows.Forms.TextBox();
            this.buttonSpeak = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonVary = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // speechBox
            // 
            this.speechBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.speechBox.Location = new System.Drawing.Point(13, 13);
            this.speechBox.Multiline = true;
            this.speechBox.Name = "speechBox";
            this.speechBox.Size = new System.Drawing.Size(259, 189);
            this.speechBox.TabIndex = 0;
            // 
            // buttonSpeak
            // 
            this.buttonSpeak.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSpeak.Location = new System.Drawing.Point(13, 220);
            this.buttonSpeak.Name = "buttonSpeak";
            this.buttonSpeak.Size = new System.Drawing.Size(75, 23);
            this.buttonSpeak.TabIndex = 1;
            this.buttonSpeak.Text = "Speak";
            this.buttonSpeak.UseVisualStyleBackColor = true;
            this.buttonSpeak.Click += new System.EventHandler(this.buttonSpeak_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExit.Location = new System.Drawing.Point(197, 220);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 2;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonVary
            // 
            this.buttonVary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonVary.Location = new System.Drawing.Point(105, 220);
            this.buttonVary.Name = "buttonVary";
            this.buttonVary.Size = new System.Drawing.Size(75, 23);
            this.buttonVary.TabIndex = 3;
            this.buttonVary.Text = "Vary volume";
            this.buttonVary.UseVisualStyleBackColor = true;
            this.buttonVary.Click += new System.EventHandler(this.buttonVary_Click);
            // 
            // Speak
            // 
            this.AcceptButton = this.buttonExit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.buttonVary);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonSpeak);
            this.Controls.Add(this.speechBox);
            this.Name = "Speak";
            this.Text = "Speak";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox speechBox;
        private System.Windows.Forms.Button buttonSpeak;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonVary;
    }
}

