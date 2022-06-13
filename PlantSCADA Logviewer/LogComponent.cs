using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class LogComponent : ViewModelBase, INodeLog
    {
       public LogComponent(string name, List<LogGroup> logfiles)
        {
            Name = name;
            Children = new List<INodeLog>();
            Children.AddRange(logfiles);
        }
        ComponentType _type;
        string _name;
        List<INodeLog> _logFiles;
        public string Name { get => _name; set => _name = value; }
        public List<INodeLog> Children { get => _logFiles; set => _logFiles = value; }
        public ComponentType Type { get => _type; set => _type = value; }
    }
}
