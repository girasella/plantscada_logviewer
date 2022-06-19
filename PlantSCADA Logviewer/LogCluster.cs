using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class LogCluster : ViewModelBase, INodeLog
    {
        string _name;


        List<INodeLog> _logComponents;
        public string Name { get => _name; set => _name = value; }
        public List<INodeLog> Children { get => _logComponents; set => _logComponents = value; }

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
    }
}
