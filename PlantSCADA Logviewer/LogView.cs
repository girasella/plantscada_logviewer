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


        public LogView()
        {
            Name = "";
        }
        public LogView(string name)
        {
            Name = name;
        }

        public LogView(string name, IEnumerable<LogEntry> collection) : base(collection, new LogEntryComparer(),false)
        {
            Name = name;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public void Merge(IEnumerable<LogEntry> other)
        {
            this.AddRange(other);
            this.Sort();
        }

    }
}
