using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlantSCADA_Logviewer
{
    internal class LogComponent : ViewModelBase, INodeLog
    {

        public LogComponent(string name, ComponentType type, INodeLog par)
        {
            Name = name;
            Type = type;
            Children = new List<INodeLog>();
            Parent = par;
        }

        public LogComponent()
        {
            Name = "LogComponent Example ";
            Children = new List<INodeLog>();
        }



        ComponentType _type;
        string _name;
        List<INodeLog> _logGroups;
        INodeLog _parent;
        public string Name { get => _name; set => _name = value; }
        public List<INodeLog> Children { get => _logGroups; set => _logGroups = value; }
        public ComponentType Type { get => _type; set => _type = value; }

        public INodeLog Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }

        }
        public bool Selected
        {
            get
            {
                return false;
            }
            set { }
        }
    }
}
