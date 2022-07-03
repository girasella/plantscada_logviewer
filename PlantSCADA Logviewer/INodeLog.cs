using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlantSCADA_Logviewer
{
    public interface INodeLog : INotifyPropertyChanged
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

       public string ToString()
        {
            return Name;
        }
        
        public bool Selected { get; set; }



    }
}
