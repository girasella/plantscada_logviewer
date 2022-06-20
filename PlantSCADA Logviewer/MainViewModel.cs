using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
            LogComponent lc1 = new LogComponent("alarmSrv",ComponentType.Alarm);
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
            DirectoryInfo logsDirectory = new DirectoryInfo(logDir);
            Dictionary<string, LogCluster> clusterMap = new Dictionary<string, LogCluster>();
            Dictionary<string, LogComponent> componentMap = new Dictionary<string, LogComponent>();

            IEnumerable<FileInfo> files =  logsDirectory.EnumerateFiles();

            foreach (FileInfo file in files)
            {
                string fName = file.Name.ToLower();

                if (fName == "syslog.dat")
                {


                    continue;
                }

                if (fName == "tracelog.dat")
                {

                    continue;
                }

                if (fName == "debug.log")
                {

                    continue;
                }

                if (fName == "params.dat")
                {


                    continue;
                }

                if (fName == "reloadlog.dat")
                {

                    continue;
                }

                var fileNameParts = fName.Split('.');
                if (fileNameParts.Length != 5)
                    continue;
                if (fileNameParts[4] != "dat" && fileNameParts[4] != "log")
                    continue;

                LogFile lFile = new LogFile(file);

                string logGroupType = fileNameParts[0];
                string logComponentType = fileNameParts[1];
                string clusterName = fileNameParts[2]; 
                string logComponentName = fileNameParts[3];

                ComponentType? cType = ComponentDictionary.GetComponent(logComponentType);
                if (cType == null)
                    continue;

                LogType? lType = LogTypeDictionary.GetLogType(logGroupType);
                if (lType == null)
                    continue;

                if (!clusterMap.ContainsKey(clusterName))
                    clusterMap.Add(clusterName, new LogCluster(clusterName));

                LogCluster currentCluster = clusterMap[clusterName];

                LogComponent currentComponent= (LogComponent) currentCluster.Children.FirstOrDefault(x => ((LogComponent)x).Name == logComponentName);

                if (currentComponent == null)
                {
                    currentComponent = new LogComponent(logComponentName, (ComponentType)cType);
                    currentCluster.Children.Add(currentComponent);
                }

                LogGroup currentGroup = (LogGroup)currentComponent.Children.FirstOrDefault(x => ((LogGroup)x).Type == lType);

                if (currentGroup == null)
                {
                    currentGroup = new LogGroup(logComponentType, (LogType)lType);
                    currentComponent.Children.Add(currentGroup);
                }

                currentGroup.LogFiles.Add(lFile);

            }
        }

    }
}
