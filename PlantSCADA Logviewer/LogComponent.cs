using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class LogComponent : ViewModelBase, INodeLog
    {

        public LogComponent(string name, ComponentType type)
        {
            Name = name;
            Type = type;
            Children = new List<INodeLog>();
        }

        public LogComponent()
        {
            Name = "LogComponent Example ";
            Children = new List<INodeLog>();
        }
        ComponentType _type;
        string _name;
        List<INodeLog> _logGroups;
        public string Name { get => _name; set => _name = value; }
        public List<INodeLog> Children { get => _logGroups; set => _logGroups = value; }
        public ComponentType Type { get => _type; set => _type = value; }
    }
}
