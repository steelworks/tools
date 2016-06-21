using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backup
{
    class BackupItem
    {
        public string Path { get; set; }
        public TimeSpan TimeToBackup { get; set; }
        public long ArchiveSize { get; set; }
        public string Status { get; set; }

        public BackupItem(string aPath)
        {
            Path = aPath;
            TimeToBackup = new TimeSpan(0, 0, 0);
            ArchiveSize = 0;
            Status = string.Empty;
        }

        public override string ToString()
        {
            string result = Path;

            if (TimeToBackup.TotalSeconds > 0)
            {
                result += (": " + TimeToBackup.ToString(@"mm\:ss"));
            }

            if (ArchiveSize > 0)
            {
                result += (", " + ArchiveSize/1000000 + " MB" );
            }

            return result + " " + Status;
        }
    }
}
