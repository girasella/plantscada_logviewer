using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class MainViewModel : ViewModelBase
    {
        string _logsPath;

        public string LogsPath
        {
            get
            {
                return _logsPath;
            }
            set { 
                _logsPath = value; 
                OnPropertyChanged();
            
            }
        }
    }
}
