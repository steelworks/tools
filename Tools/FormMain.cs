using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Management;
using Ionic.Zip;

namespace Backup
{
    public partial class FormMain : Form
    {
        Thread iThread;
        List<BackupItem> iBackupItems;
        int iNumFilesBackedUp = 0;
        string iBackupMedia;        // C:\, D:\ - the drive to hold the archive
        string iBackupArchivePath;  // Path to the folder containing the new archive
        DateTime iStartTime;

        public FormMain()
        {
            InitializeComponent();

            // Append CVS version to the main form title
            string version = "$Name: Rev_1_001 $";
            string[] components = version.Split(' ');
            if ((components.Length >= 3) && !version.Contains("1_001"))
            {
                this.Text += (" " + components[1]);
            }
            else
            {
                this.Text += " beta";
            }

            // Make the form appear
            Application.DoEvents();

            // Get the list of folders to be backed up
            if (GetBackupList())
            {
                // Verify that the list of folders to be backed up is valid
                if (!CheckBackupList())
                {
                    labelComplete.Visible = true;
                    labelComplete.Text = "Folders not found";
                }
                else if (!FindBackupMedia())
                {
                    labelComplete.Visible = true;
                    labelComplete.Text = "Failed to identify backup media";
                }
                else
                {
                    iStartTime = DateTime.Now;

                    // Cycle the root folders on the backup media, creating a new iBackupArchivePath folder
                    CycleBackupMedia();

                    // Run the actual backup in the background
                    ThreadStart threadStart = new ThreadStart(BackgroundThread);
                    iThread = new Thread(threadStart);
                    iThread.Start();
                    labelComplete.Visible = true;
                    labelComplete.Text = "Backing up to " + iBackupMedia;

                    timerControl.Enabled = true;
                }
            }
            else
            {
                labelComplete.Visible = true;
                labelComplete.Text = "Failed to load backup list";
            }
        }

        /// <summary>
        /// Set the iBackupMedia variable to identify the disk to which backup is made.
        /// The first disk that contains a "BackupMedia.txt" file is the backup disk.
        /// If no such disk is found, then create it.
        /// </summary>
        /// <returns>True if a suitable disk is found</returns>
        private bool FindBackupMedia()
        {
            const string markerFilename = "BackupMedia.txt";
            iBackupMedia = string.Empty;

            FormDriveSelector driveSelector = new FormDriveSelector();

            for (char drive = 'C'; drive <= 'Z'; drive++)
            {
                DirectoryInfo driveRoot = new DirectoryInfo(drive + @":\");
                if (driveRoot.Exists)
                {
                    string markerFile = Path.Combine(driveRoot.FullName, markerFilename);
                    if (File.Exists(markerFile))
                    {
                        // Success, found the backup media, no need to loop any further
                        iBackupMedia = driveRoot.FullName;
                        break;
                    }
                    else
                    {
                        // In case we need to mark a new disk, remember the available disks
                        driveSelector.Add( driveRoot.FullName );
                    }
                }
            }

            // Return true if we found backup media
            if (iBackupMedia.Length > 0)
            {
                return true;
            }
            else
            {
                // No suitable media found: let the user select a drive
                if (driveSelector.ShowDialog() == DialogResult.OK)
                {
                    iBackupMedia = driveSelector.Which;
                    File.Create( Path.Combine( iBackupMedia, markerFilename ) );
                    return true;
                }
                else
                {
                    // User does not want to select a drive
                    return false;
                }
            }
        }

        /// <summary>
        /// Identify the backup_list.txt file that contains the list of folders to be backed up.
        /// Load the path to each folder into iBackupItems, and populate the filesListBox on the main form.
        /// </summary>
        /// <returns>True if backup list successfully loaded and is not empty</returns>
        private bool GetBackupList()
        {
            string backupList = Properties.Settings.Default.BackupList;
            if (!File.Exists(backupList))
            {
                return false;
            }

            iBackupItems = new List<BackupItem>();
            using (StreamReader backupReader = new StreamReader(backupList))
            {
                string backupPath;
                while (((backupPath = backupReader.ReadLine()) != null) &&
                       (backupPath.Length > 0))
                {
                    iBackupItems.Add(new BackupItem(backupPath));
                }
            }

            filesListBox.Items.AddRange(iBackupItems.ToArray());
            return (iBackupItems.Count > 0);
        }

        private bool SaveBackupList()
        {
            string backupList = Properties.Settings.Default.BackupList;

            try
            {
                using (StreamWriter backupWriter = new StreamWriter(backupList))
                {
                    foreach (BackupItem backupItem in iBackupItems)
                    {
                        backupWriter.WriteLine(backupItem.Path);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Failed to rewrite backup list");
                return false;
            }

            // Refresh the display with the saved backup list
            filesListBox.Items.Clear();
            filesListBox.Items.AddRange(iBackupItems.ToArray());
            return true;
        }

        /// <summary>
        /// Check for existence of each folder to be backed up.
        /// If any folder does not exist, show a MessageBox warning the user.
        /// </summary>
        /// <returns>True if all folders in the backup list exist</returns>
        private bool CheckBackupList()
        {
            string badList = string.Empty;
            foreach (BackupItem backupItem in iBackupItems)
            {
                if (!Directory.Exists(backupItem.Path))
                {
                    badList += (backupItem + "\n");
                }
            }

            if (badList.Length > 0)
            {
                MessageBox.Show(badList, "Folder not found");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Cycle the archive folders on the backup media 
        /// </summary>
        private void CycleBackupMedia()
        {
            // Delete any surplus archives
            string[] previousBackups = Directory.GetDirectories(iBackupMedia, "backup*");
            if (previousBackups.Length >= Properties.Settings.Default.NumArchives)
            {
                Array.Sort(previousBackups);
                for (int i = 0; i <= (previousBackups.Length - Properties.Settings.Default.NumArchives); i++)
                {
                    Directory.Delete(previousBackups[i], true);
                }
            }

            // Create the folder for the new archive
            iBackupArchivePath = Path.Combine(iBackupMedia, "backup" + DateTime.Now.ToString("yyyyMMdd"));

            // Just in case a backup has already been taken today, append an "a" to make the new path unique
            while (Directory.Exists(iBackupArchivePath))
            {
                iBackupArchivePath += "a";
            }

            Directory.CreateDirectory(iBackupArchivePath);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Periodic refresh of the form showing the current state of the backup operation,
        /// which is running in the background.
        /// </summary>
        private void timerControl_Tick(object sender, EventArgs e)
        {
            TimeSpan lapsedTime = DateTime.Now - iStartTime;

            if (iThread.ThreadState == ThreadState.Stopped)
            {
                labelComplete.Text = 
                    string.Format("Complete after {0:00}:{1:00}",
                                  lapsedTime.Minutes, lapsedTime.Seconds);
                filesListBox.Items.Add(string.Empty);
                foreach (string drive in GetDrives())
                {
                    // List the stats for each drive for which backups are taken
                    filesListBox.Items.Add(DiskStats(drive));
                }
                filesListBox.Items.Add(DiskStats(iBackupMedia.Remove(2)));
                timerControl.Enabled = false;
            }
            else
            {
                labelComplete.Text = 
                    string.Format("{0:00}:{1:00} Backed up {2} files", 
                                  lapsedTime.Minutes, lapsedTime.Seconds, 
                                  iNumFilesBackedUp);
            }

            // Force redraw to reflect progress
            filesListBox.Invalidate();
        }

        // Get list of drives from which backups are taken
        private List<string> GetDrives()
        {
            List<string> drives = new List<string>();
            foreach (BackupItem backupItem in iBackupItems)
            {
                string thisDrive = backupItem.Path.Substring(0, 2);
                if ( (thisDrive[1] == ':') && !drives.Contains(thisDrive))
                {
                    drives.Add(thisDrive);
                }
            }

            return drives;
        }

        private string DiskStats(string aDrive)
        {
            string managementDescriptor = 
                string.Format("win32_logicaldisk.deviceid=\"{0}\"", aDrive);
            ManagementObject disk = new ManagementObject(managementDescriptor);
            disk.Get();
            long totalMb = long.Parse(disk["Size"].ToString()) / 1000000;
            long freeMb = long.Parse(disk["FreeSpace"].ToString()) / 1000000;
            int pcFree = (int)((freeMb * 100) / totalMb);
            return string.Format("Drive {0} free space: {1} / {2} MB ({3}%)", 
                                 aDrive, freeMb, totalMb, pcFree);
        }

        /// <summary>
        /// Do the backup in the background
        /// </summary>
        private void BackgroundThread()
        {
            // Create a zip of each folder to be backed up, in the Recent folder of the backup media
            foreach (BackupItem backupItem in iBackupItems)
            {
                string backupFile = Path.GetFileName(backupItem.Path) + ".zip";
                string backupPath = Path.Combine(iBackupArchivePath, backupFile);

                try
                {
                    using (ZipFile zip = new ZipFile(backupPath))
                    {
                        if (Properties.Settings.Default.Password.Length > 0)
                        {
                            zip.Password = Properties.Settings.Default.Password;
                        }

                        DateTime start = DateTime.Now;
                        zip.AddDirectory(backupItem.Path);
                        zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");
                        zip.Save();
                        DateTime end = DateTime.Now;
                        backupItem.TimeToBackup = (end - start);
                        backupItem.ArchiveSize = new FileInfo(backupPath).Length;
                        backupItem.Status = "OK";
                    }
                }
                catch (Exception ex)
                {
                    backupItem.Status = ex.Message;
                }

                iNumFilesBackedUp++;
            }
        }

        private void filesListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Set the DrawMode property to draw fixed sized items.
            filesListBox.DrawMode = DrawMode.OwnerDrawFixed;

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();

            if (e.Index < 0)
            {
                // Nothing in the list
                return;
            }

            // Define the default color of the brush as black.
            Brush myBrush = Brushes.Black;

            // Determine the color of the brush to draw each item based on the index of the item to draw.
            Font requiredFont = new Font(e.Font, FontStyle.Regular);
            if (e.Index < iNumFilesBackedUp)
            {
                myBrush = Brushes.Green;        // Backup complete
            }
            else if (e.Index == iNumFilesBackedUp)
            {
                myBrush = Brushes.Orange;       // Backup in progress
            }
            else if (e.Index < iBackupItems.Count)
            {
                myBrush = Brushes.Red;          // Backup not started
            }
            else
            {
                // Beyond the list of files: this is status info
                myBrush = Brushes.Black;
            }

            // Draw the current item text based on the current Font and the custom brush settings.
            e.Graphics.DrawString(filesListBox.Items[e.Index].ToString(), requiredFont, myBrush, 
                                  e.Bounds, StringFormat.GenericDefault);

            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        private void filesListBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point p = new Point(e.X, e.Y);
                filesListBox.SelectedIndex = filesListBox.IndexFromPoint(p);
            }
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // New folder to be backed up may be in the vicinity of the existing
            // first in the list
            folderBrowserDialog.SelectedPath = iBackupItems[0].Path;
            DialogResult res = folderBrowserDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                // Add item to in-memory backup list
                string newFolder = folderBrowserDialog.SelectedPath;
                iBackupItems.Add(new BackupItem(newFolder));

                // Save revised backup list to file
                SaveBackupList();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = string.Format("Remove {0} from backup list?", 
                                           filesListBox.SelectedItem);
            DialogResult res = 
                MessageBox.Show(message, "Backup", MessageBoxButtons.OKCancel);
            if (res == DialogResult.OK)
            {
                // Remove item from in-memory backup list
                string pathToRemove = filesListBox.SelectedItem.ToString();
                for (int i = 0; i < iBackupItems.Count; i++)
                {
                    if (iBackupItems[i].Path == pathToRemove)
                    {
                        iBackupItems.RemoveAt(i);
                        break;
                    }
                }

                // Save revised backup list to file
                SaveBackupList();
            }
        }
    }
}
