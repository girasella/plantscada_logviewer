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

        public INodeLog Parent { get; set; }

        public void AddChild(INodeLog child);
        
        public void RemoveChild(INodeLog child);

        
        public bool Selected { get; set; }

        internal void UpdateSelectedProperty();

    }
}
