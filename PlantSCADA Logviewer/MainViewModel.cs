using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlantSCADA_Logviewer
{
    internal class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {
            FilterTime = new TimeFilter();
            FilterSetup = new DelegateCommand<int>((par) => FilterTime.FilterFromNow(par));
            LogsPath = "C:\\ProgramData\\Aveva\\Citect SCADA 2018 R2\\Logs";
        }
        string _logsPath="";

        List<INodeLog> _treeElems;

        TimeFilter _filterTime;

        ICommand _filterSetup;

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

        public TimeFilter FilterTime
        {
            get { return _filterTime; }

            set { 
                _filterTime = value;
                OnPropertyChanged(); 
            }

        }

        public ICommand FilterSetup { get => _filterSetup; set => _filterSetup = value; }
        internal List<INodeLog> TreeElems { get => _treeElems; set => _treeElems = value; }


    }
}
