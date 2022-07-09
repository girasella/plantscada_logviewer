using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PlantSCADA_Logviewer
{
    internal class LogView : SortableObservableCollection<LogEntry>, IEnumerable<LogEntry>
    {
        string _name;

        List<LogGroup> groups;

        TimeFilter _timeInterval;

        public LogView(TimeFilter tf)
        {
            Name = "";
            groups = new List<LogGroup>();
            TimeInterval = new TimeFilter(tf.DateStart, tf.DateEnd);
        }

        //public LogView(string name)
        //{
        //    Name = name;
        //    groups = new List<LogGroup>();
        //}

        public LogView(string name, IEnumerable<LogEntry> collection) : base(collection, new LogEntryComparer(),false)
        {
            Name = name;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        internal TimeFilter TimeInterval { get => _timeInterval; set => _timeInterval = value; }

        public void ApplyTimeFilter(DateTime start, DateTime end)
        {
            TimeInterval.DateStart = start;
            TimeInterval.DateEnd = end;
            this.Clear();
            foreach (LogGroup lGroup in groups)
            {
                this.AddRange(lGroup.LogEntries.Where(x => x.Date >= TimeInterval.DateStart && x.Date < TimeInterval.DateEnd));
            }
        }

        internal void AddGroup(LogGroup lGroup)
        {
            groups.Add(lGroup);
            this.AddRange(lGroup.LogEntries.Where(x=>x.Date >= TimeInterval.DateStart && x.Date < TimeInterval.DateEnd));           
        }

        internal void RemoveGroup(LogGroup lGroup)
        {
            groups.Remove(lGroup);
            this.RemoveAll(x => x.SourceNode == lGroup);
        }

    }
}
