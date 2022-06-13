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
    }
}
