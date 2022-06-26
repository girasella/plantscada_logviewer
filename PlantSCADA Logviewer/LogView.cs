using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class LogView : List<LogEntry>
    {
        string _name;

        public LogView(string name)
        {
            Name = name;
        }

        public LogView(string name, IEnumerable<LogEntry> collection) : base(collection)
        {
            Name = name;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public void Merge(LogView other)
        {
            this.AddRange(other);
            this.Sort();

        }

    }
}
