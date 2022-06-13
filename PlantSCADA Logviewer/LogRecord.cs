using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class LogRecord : ViewModelBase
    {
        string _message;

        public DateTime _date;
        public LogRecord(DateTime date, string message)
        {
            Date = date;
            Message = message;
        }

        public DateTime Date { get => _date; set => _date = value; }
        public string Message { get => _message; set => _message = value; }
    }
}
