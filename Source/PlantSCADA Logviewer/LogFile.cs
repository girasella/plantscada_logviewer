using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Diagnostics;

namespace PlantSCADA_Logviewer
{
    internal class LogFile : ViewModelBase
    {
        FileInfo _file;
        List<LogEntry> _logEntries;
        LogGroup _source;
        ICommand _openFile;

        public LogFile(FileInfo file, LogGroup logGroup)
        {
            _file = file;
            Source = logGroup;
            _openFile = new DelegateCommand(() => openFile());
            
        }

        public ICommand OpenFile
        {
            get { return _openFile; }
        }
        public IEnumerable<LogEntry> Load(DateTime start, DateTime end)
        {
            if (LogEntries == null)
            {
                LogEntries = ReadFile();
            }
            var retValue = LogEntries.Where(x => x.Date > start && x.Date < end);

            return retValue == null ? new List<LogEntry>() : retValue;
        }

        private List<LogEntry> ReadFile()
        {
            List<LogEntry> retValue = new List<LogEntry>();
            string[] lines = File.ReadAllLines(_file.FullName);

            foreach(string line in lines)
            {
                if (line.Length < 30) 
                    continue;
                string dateString = line.Substring(0, 30);

                DateTime dt;

                if (!DateTime.TryParseExact(dateString, "yyyy-MM-dd HH:mm:ss.fff\tzzz", CultureInfo.CurrentUICulture, DateTimeStyles.None, out dt))
                    continue;

                string msg = line.Substring(31, line.Length - 31);
                msg = Regex.Replace(msg, " {2,}", " ");
                msg = msg.Replace("\t"," ");
                retValue.Add(new LogEntry(dt, msg,this.Source));
            }

            return retValue; 

        }

        string _fileName;
        DateTime _start, _end;

        public string FileName { get => _file.Name; }
        public DateTime Start { get => _start; set => _start = value; }
        public DateTime End { get => _end; set => _end = value; }
        public LogGroup Source { get => _source; set => _source = value; }
        internal List<LogEntry> LogEntries {
            get
            {
                if (_logEntries == null)
                {
                    _logEntries = ReadFile();
                }
                return _logEntries;
            }
            set => _logEntries = value; 
       
        }

        void openFile()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = _file.FullName;
            psi.UseShellExecute = true;
            Process.Start(psi);
        }
    }
}
