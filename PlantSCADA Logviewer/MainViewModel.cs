using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            LogViews = new ObservableCollection<LogView>();
            LogViews.Add(new LogView("LogView1"));
            TreeElems = new ObservableCollection<INodeLog>();
            LogCluster cl1 = new LogCluster("Cluster1");
            LogGroup lg1 = new LogGroup("syslog", LogType.Sys);            
            LogComponent lc1 = new LogComponent("alarm", new List<LogGroup>());
            cl1.Children.Add(lc1);
            lc1.Children.Add(lg1);

            TreeElems.Add(cl1);

        }
        string _logsPath="";

        ObservableCollection<INodeLog> _treeElems;

        TimeFilter _filterTime;

        ICommand _filterSetup;

        ObservableCollection<LogView> _logViews;


        public ObservableCollection<LogView> LogViews
        {
            get
            {
                return _logViews;
            }
            set
            {
                _logViews = value;
            }
        }
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
        public ObservableCollection<INodeLog> TreeElems
        {
            get => _treeElems;
            set
            {
                _treeElems = value;
                OnPropertyChanged();
            }
        }


        public void ScanLogDirectory (string logDir)
        {

        }

    }
}
