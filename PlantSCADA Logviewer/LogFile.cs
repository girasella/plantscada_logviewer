using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace PlantSCADA_Logviewer
{
    internal class LogFile : ViewModelBase
    {
        FileInfo _file;
        List<LogEntry> _logEntries;
        LogGroup _source;

        public LogFile(FileInfo file, LogGroup logGroup)
        {
            FileName = file.FullName;
            _file = file;
            Source = logGroup;      
            
        }

        public IEnumerable<LogEntry> Load(DateTime start, DateTime end)
        {
            if (_logEntries == null)
            {
                _logEntries = ReadFile();
            }
            var retValue = _logEntries.Where(x => x.Date > start && x.Date < end);

            return retValue == null ? new List<LogEntry>() : retValue;
        }

        private List<LogEntry> ReadFile()
        {
            List<LogEntry> retValue = new List<LogEntry>();
            string[] lines = File.ReadAllLines(_file.FullName);

            foreach(string line in lines)
            {
                string dateString = line.Substring(0, 30);

                DateTime dt;

                if (!DateTime.TryParseExact(dateString, "yyyy-MM-dd HH:mm:ss.fff\tzzz", CultureInfo.CurrentUICulture, DateTimeStyles.None, out dt))
                    continue;

                string msg = line.Substring(31, line.Length - 31);
                msg = msg.Replace('\t', ' ');
                retValue.Add(new LogEntry(dt, msg,this.Source));
            }

            return retValue; 

        }

        string _fileName;
        DateTime _start, _end;

        public string FileName { get => _fileName; set => _fileName = value; }
        public DateTime Start { get => _start; set => _start = value; }
        public DateTime End { get => _end; set => _end = value; }
        public LogGroup Source { get => _source; set => _source = value; }
    }
}
