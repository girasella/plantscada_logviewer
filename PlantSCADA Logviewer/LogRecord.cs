using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class LogRecord : ViewModelBase, IComparer<LogRecord>
    {
        string _message;
        bool _visible;

        public DateTime _date;
        public LogRecord(DateTime date, string message)
        {
            Date = date;
            Message = message;
            Visible = true;
        }

        public DateTime Date { get => _date; set => _date = value; }
        public string Message { get => _message; set => _message = value; }
        public bool Visible { get => _visible; set => _visible = value; }

        public int Compare(LogRecord? x, LogRecord? y)
        {
            return x.Date.CompareTo(y.Date);
        }
    }
}
