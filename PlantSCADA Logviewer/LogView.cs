using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class LogView : List<LogRecord>
    {
        string _name;

        

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
