using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backup
{
    class BackupItem
    {
        public string Path { get; set; }
        public DateTime TimeOfStartOfBackup { get; set; }
        public TimeSpan TimeToBackup { get; set; }
        public uint HistoricTimeToBackup { get; set; }      // Seconds
        public long ArchiveSize { get; set; }
        public string Status { get; set; }

        public BackupItem(string aPath, uint historicSeconds = 0)
        {
            Path = aPath;
            TimeOfStartOfBackup = DateTime.Now;         // Default value, not really true
            TimeToBackup = new TimeSpan(0, 0, 0);
            HistoricTimeToBackup = historicSeconds;
            ArchiveSize = 0;
            Status = string.Empty;
        }

        public override string ToString()
        {
            string result = Path;

            // The forecast time to backup is taken from historic time, or defaults to 5 minutes if no history
            uint forecastSeconds = HistoricTimeToBackup > 0 ? HistoricTimeToBackup : (uint)TimeSpan.FromMinutes(5).TotalSeconds;

            // Progress can never be greater than 100%
            uint progressPercent = Math.Min((uint)TimeToBackup.TotalSeconds * 100 / forecastSeconds, 100);
            result += $": {progressPercent}%";

            if (TimeToBackup.TotalSeconds > 0)
            {
                result += (", " + TimeToBackup.ToString(@"mm\:ss"));
            }

            if (ArchiveSize > 0)
            {
                result += (", " + ArchiveSize/1000000 + " MB" );
            }

            return result + " " + Status;
        }
    }
}
