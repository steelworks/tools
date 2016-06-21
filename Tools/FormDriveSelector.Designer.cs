namespace Backup
{
    partial class FormDriveSelector
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
            this.iButtonOK = new System.Windows.Forms.Button();
            this.iButtonCancel = new System.Windows.Forms.Button();
            this.iListBoxDrive = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // iButtonOK
            // 
            this.iButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.iButtonOK.Location = new System.Drawing.Point(62, 231);
            this.iButtonOK.Name = "iButtonOK";
            this.iButtonOK.Size = new System.Drawing.Size(75, 23);
            this.iButtonOK.TabIndex = 0;
            this.iButtonOK.Text = "OK";
            this.iButtonOK.UseVisualStyleBackColor = true;
            // 
            // iButtonCancel
            // 
            this.iButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.iButtonCancel.Location = new System.Drawing.Point(152, 231);
            this.iButtonCancel.Name = "iButtonCancel";
            this.iButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.iButtonCancel.TabIndex = 1;
            this.iButtonCancel.Text = "Cancel";
            this.iButtonCancel.UseVisualStyleBackColor = true;
            // 
            // iListBoxDrive
            // 
            this.iListBoxDrive.FormattingEnabled = true;
            this.iListBoxDrive.Location = new System.Drawing.Point(13, 13);
            this.iListBoxDrive.Name = "iListBoxDrive";
            this.iListBoxDrive.Size = new System.Drawing.Size(259, 212);
            this.iListBoxDrive.TabIndex = 2;
            // 
            // FormDriveSelector
            // 
            this.AcceptButton = this.iButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.iButtonCancel;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.ControlBox = false;
            this.Controls.Add(this.iListBoxDrive);
            this.Controls.Add(this.iButtonCancel);
            this.Controls.Add(this.iButtonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDriveSelector";
            this.ShowInTaskbar = false;
            this.Text = "Select drive for backup media";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button iButtonOK;
        private System.Windows.Forms.Button iButtonCancel;
        private System.Windows.Forms.ListBox iListBoxDrive;
    }
}