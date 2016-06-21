using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Backup
{
    /// <summary>
    /// Allow user to select a drive
    /// </summary>
    public partial class FormDriveSelector : Form
    {
        /// <summary>
        /// Return the selected drive
        /// </summary>
        internal string Which
        {
            get
            {
                if (iListBoxDrive.Items.Count > 0)
                {
                    return iListBoxDrive.SelectedItem.ToString();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FormDriveSelector()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Make a drive available for selection
        /// </summary>
        /// <param name="aDrive"></param>
        internal void Add(string aDrive)
        {
            iListBoxDrive.Items.Add(aDrive);
            iListBoxDrive.SelectedIndex = iListBoxDrive.Items.Count - 1;
        }
    }
}
