using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class LogEntry : ViewModelBase, IComparer<LogEntry>
    {
        string _message;
        string _source;
        bool _visible;

        public DateTime _date;
        public LogEntry(DateTime date, string message, string source)
        {
            Date = date;
            Message = message;
            Visible = true;
            Source = source;
        }

        public DateTime Date { get => _date; set => _date = value; }
        public string Message { get => _message; set => _message = value; }
        public bool Visible { get => _visible; set => _visible = value; }
        public string Source { get => _source; set => _source = value; }

        public int Compare(LogEntry? x, LogEntry? y)
        {
            return x.Date.CompareTo(y.Date);
        }
    }
}
