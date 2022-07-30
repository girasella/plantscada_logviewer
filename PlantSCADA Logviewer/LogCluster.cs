using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlantSCADA_Logviewer
{
    internal class LogCluster : ViewModelBase, INodeLog
    {
        string _name;


        List<INodeLog> _logComponents;

        INodeLog _parent;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public List<INodeLog> Children { get => _logComponents; set => _logComponents = value; }

        public bool Selected
        {
            get
            {
                return Children.All(x => x.Selected);
            }
            set
            {

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

        public INodeLog Parent { 
            get => null; 
            set => _parent = null; }

        public LogCluster(string name)
        {
            Name = name;
            Children = new List<INodeLog>();
        }

        public LogCluster()
        {
            Name = "LogCluster Example";
            Children = new List<INodeLog>();
        }


        public override string ToString()
        {
            return Name;
        }

        public void UpdateSelectedProperty()
        {
            OnPropertyChanged(nameof(Selected));
            if (Parent != null)
                Parent.UpdateSelectedProperty();
        }

        public void AddChild(INodeLog child)
        {
            Children.Add(child);
        }

        public void RemoveChild(INodeLog child)
        {
            Children.Remove(child);
        }
    }
}
