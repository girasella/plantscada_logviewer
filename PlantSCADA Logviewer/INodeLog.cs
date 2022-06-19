using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal interface INodeLog
    {
        public string Name { get; set; }

        public List<INodeLog> Children { get; set; }


        public void AddChild(INodeLog child) { 
            Children.Add(child); 
        }
        public void RemoveChild(INodeLog child)
        {
            Children.Remove(child);
        }       
    }
}
