using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;

namespace PlantSCADA_Logviewer
{
    internal class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {
            FilterTime = new TimeFilter();
            InitCommands();
            LogsPath = "C:\\ProgramData\\Aveva\\Citect SCADA 2018 R2\\Logs";
            LogViewer = new LogView(_logsPath);
            LogViews = new ObservableCollection<LogView>();
            LogViews.Add(new LogView("LogView1"));
            TreeElems = new ObservableCollection<INodeLog>();
            LogCluster cl1 = new LogCluster("Cluster1");
            LogComponent lc1 = new LogComponent("alarmSrv", ComponentType.Alarm,cl1);
            LogGroup lg1 = new LogGroup("syslog", LogType.Sys,lc1);            

            cl1.Children.Add(lc1);
            lc1.Children.Add(lg1);
            TreeElems.Add(cl1);
        }
        public MainViewModel(bool designer)
        {
            FilterTime = new TimeFilter();
            InitCommands();
            LogsPath = "C:\\ProgramData\\Aveva\\Citect SCADA 2018 R2\\Logs";
            LogViews = new ObservableCollection<LogView>();
            LogViewer = new LogView(_logsPath);
            TreeElems = new ObservableCollection<INodeLog>();
        }

        static MainViewModel _instance;

        string _logsPath="";

        ObservableCollection<INodeLog> _treeElems;

        TimeFilter _filterTime;

        ICommand _filterSetup, _setTree, _browseFolders;

        ObservableCollection<LogView> _logViews;

        LogView _logView;

        internal static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainViewModel(true);
                return _instance;
            }
        }
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

        public ICommand SetTree { 
            get => _setTree; 
            set => _setTree = value; }
        public ICommand BrowseFolders { get => _browseFolders; set => _browseFolders = value; }
        public LogView LogViewer
        { 
            get
            {
                return _logView;
            }
            set
            {
                _logView = value;
                OnPropertyChanged();
            }
        
        }

        void BrowseDirs()
        {
            var dialog = new FolderBrowserDialog();

            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK) 
                LogsPath = dialog.SelectedPath;

        }

        void ScanLogDirectory (string logDir)
        {
            DirectoryInfo logsDirectory = new DirectoryInfo(logDir);
            Dictionary<string, LogCluster> clusterMap = new Dictionary<string, LogCluster>();
            Dictionary<string, LogComponent> componentMap = new Dictionary<string, LogComponent>();

            IEnumerable<FileInfo> files =  logsDirectory.EnumerateFiles();

            LogComponent clientComponent = new LogComponent("Client", ComponentType.Client,null);

            foreach (FileInfo file in files)
            {
                if (file.Extension != ".dat" && file.Extension != ".log")
                    continue;

                string fName = file.Name.ToLower();
                              
                string[] fileNameParts = fName.Split('.');

                string logGroupType = fileNameParts[0];
                
                LogType? lType = LogTypeDictionary.GetLogType(logGroupType);

                if (fileNameParts.Length == 2)
                {
                    LogGroup cGroup = (LogGroup)clientComponent.Children.FirstOrDefault(x => ((LogGroup)x).Type == lType);

                    if (lType == null)                    
                        continue;                    

                    if (cGroup == null)
                    {
                        cGroup = new LogGroup(logGroupType, (LogType)lType,clientComponent);
                        clientComponent.Children.Add(cGroup);
                    }
                    LogFile lgFile = new LogFile(file, cGroup.SourcePath);
                    cGroup.LogFiles.Add(lgFile);
                    continue;
                }                
                if (fileNameParts.Length != 5)
                    continue;

                string logComponentType = fileNameParts[1];
                string clusterName = fileNameParts[2]; 
                string logComponentName = fileNameParts[3];

                ComponentType? cType = ComponentDictionary.GetComponent(logComponentType);
                if (cType == null)
                    continue;

                if (!clusterMap.ContainsKey(clusterName))
                    clusterMap.Add(clusterName, new LogCluster(clusterName));

                LogCluster currentCluster = clusterMap[clusterName];

                LogComponent currentComponent= (LogComponent) currentCluster.Children.FirstOrDefault(x => ((LogComponent)x).Name == logComponentName);

                if (currentComponent == null)
                {
                    currentComponent = new LogComponent(logComponentName, (ComponentType)cType,currentCluster);
                    currentCluster.Children.Add(currentComponent);
                }

                LogGroup currentGroup = (LogGroup)currentComponent.Children.FirstOrDefault(x => ((LogGroup)x).Type == lType);

                if (currentGroup == null)
                {
                    currentGroup = new LogGroup(logGroupType, (LogType)lType,currentComponent);
                    currentComponent.Children.Add(currentGroup);
                }
                LogFile lFile = new LogFile(file,currentGroup.SourcePath);
                currentGroup.LogFiles.Add(lFile);
            }


            TreeElems.Add(clientComponent);
            foreach (var elem in clusterMap)
                TreeElems.Add(elem.Value);
            
        }


        private void InitCommands()
        {
            FilterSetup = new DelegateCommand<int>((par) => FilterTime.FilterFromNow(par));
            SetTree = new DelegateCommand(() => ScanLogDirectory(LogsPath));
            BrowseFolders = new DelegateCommand(() => BrowseDirs());
        }
    
        public void UpdateLogView()
        {
            OnPropertyChanged(nameof(LogViewer));
        }
    
    }
}
