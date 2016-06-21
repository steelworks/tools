namespace Backup
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
            this.labelComplete = new System.Windows.Forms.Label();
            this.buttonExit = new System.Windows.Forms.Button();
            this.timerControl = new System.Windows.Forms.Timer(this.components);
            this.filesListBox = new System.Windows.Forms.ListBox();
            this.contextMenuBackupList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.contextMenuBackupList.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelComplete
            // 
            this.labelComplete.AutoSize = true;
            this.labelComplete.Location = new System.Drawing.Point(22, 376);
            this.labelComplete.Name = "labelComplete";
            this.labelComplete.Size = new System.Drawing.Size(90, 13);
            this.labelComplete.TabIndex = 0;
            this.labelComplete.Text = "Backup complete";
            this.labelComplete.Visible = false;
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(504, 406);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 1;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // timerControl
            // 
            this.timerControl.Interval = 1000;
            this.timerControl.Tick += new System.EventHandler(this.timerControl_Tick);
            // 
            // filesListBox
            // 
            this.filesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filesListBox.ContextMenuStrip = this.contextMenuBackupList;
            this.filesListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.filesListBox.FormattingEnabled = true;
            this.filesListBox.Location = new System.Drawing.Point(25, 13);
            this.filesListBox.Name = "filesListBox";
            this.filesListBox.Size = new System.Drawing.Size(554, 342);
            this.filesListBox.TabIndex = 0;
            this.filesListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.filesListBox_DrawItem);
            this.filesListBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.filesListBox_MouseDown);
            // 
            // contextMenuBackupList
            // 
            this.contextMenuBackupList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextMenuBackupList.Name = "contextMenuBackupList";
            this.contextMenuBackupList.Size = new System.Drawing.Size(134, 48);
            // 
            // addNewToolStripMenuItem
            // 
            this.addNewToolStripMenuItem.Name = "addNewToolStripMenuItem";
            this.addNewToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.addNewToolStripMenuItem.Text = "Add new ...";
            this.addNewToolStripMenuItem.Click += new System.EventHandler(this.addNewToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Select path to add to backup list";
            this.folderBrowserDialog.ShowNewFolderButton = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 441);
            this.Controls.Add(this.filesListBox);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.labelComplete);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "Steelworks Backup";
            this.contextMenuBackupList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelComplete;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Timer timerControl;
        private System.Windows.Forms.ListBox filesListBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuBackupList;
        private System.Windows.Forms.ToolStripMenuItem addNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

