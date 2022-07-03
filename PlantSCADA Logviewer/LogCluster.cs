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
            get {
                return false;
            }
            set { }

        }
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
    }
}
