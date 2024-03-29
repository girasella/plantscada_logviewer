﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class LogEntry : ViewModelBase, IComparable<LogEntry>
    {
        string _message;
        LogGroup _sourceNode;
        bool _visible;

        public DateTime _date;
        public LogEntry(DateTime date, string message, LogGroup source)
        {
            Date = date;
            Message = message;
            Visible = true;
            SourceNode = source;
        }

        public DateTime Date { get => _date; set => _date = value; }
        public string Message { get => _message; set => _message = value; }
        public bool Visible { get => _visible; set => _visible = value; }
        
        public LogGroup SourceNode
        {
            get => _sourceNode;
            set => _sourceNode = value;
        }

        public string Source
        {
            get
            {
                return SourceNode != null ? SourceNode.SourcePath : "";
            }
        }

        public int CompareTo(LogEntry? other)
        {
            return this.Date.CompareTo(other.Date);
        }
    }


    internal class LogEntryComparer : IComparer<LogEntry>
    {
        public int Compare(LogEntry? x, LogEntry? y)
        {
            return x.Date.CompareTo(y.Date);
        }
    }
}
