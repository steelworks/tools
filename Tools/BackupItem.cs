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
            // The forecast time to backup is taken from historic time, or defaults to 5 minutes if no history
            uint forecastSeconds = HistoricTimeToBackup > 0 ? HistoricTimeToBackup : (uint)TimeSpan.FromMinutes(5).TotalSeconds;

            // Progress in terms of tenths complete
            string progressGauge = string.Empty;
            uint progressTenths = Math.Min((uint)Math.Round(TimeToBackup.TotalSeconds * 10 / forecastSeconds), 10);
            for (var tenth = 1; tenth <= progressTenths; tenth++) progressGauge += "*";
            for (var tenth = progressTenths; tenth < 10; tenth++) progressGauge += "-";
            string result = $"[{progressGauge}] {Path}";

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
