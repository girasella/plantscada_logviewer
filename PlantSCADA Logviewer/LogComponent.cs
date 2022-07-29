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
        bool _selected;
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
                return Children.All(x=>x.Selected);
            }
            set {

                if (value && !Children.All(x => x.Selected))
                {
                    foreach (var child in Children)
                    {
                        child.Selected = true;
                    }
                }
                else
                {
                    if (!value)
                    {
                        foreach (var child in Children)
                        {
                            child.Selected = false;
                        }
                    }
                }

                Parent?.UpdateSelectedProperty();

            }
        }

        public void UpdateSelectedProperty()
        {
            OnPropertyChanged(nameof(Selected));
            if (Parent != null)
                Parent.UpdateSelectedProperty();
        }
    }
}
