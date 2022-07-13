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
using System.Windows.Data;

namespace PlantSCADA_Logviewer
{
    internal class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {
            FilterTime = new TimeFilter();
            InitCommands();
            InitTimeFilterChoices();
            LogsPath = "C:\\ProgramData\\Aveva\\Citect SCADA 2018 R2\\Logs";
            LogViewer = new LogView(FilterTime);
            ViewSource.Source = LogViewer;
            TreeElems = new ObservableCollection<INodeLog>();
            LogCluster cluster = new LogCluster("Cluster1");
            LogComponent component = new LogComponent("alarmSrv", ComponentType.Alarm,cluster);
            LogGroup group = new LogGroup("syslog", LogType.Sys,component);            

            cluster.Children.Add(component);
            component.Children.Add(group);
            TreeElems.Add(cluster);
        }
        public MainViewModel(bool designer)
        {
            FilterTime = new TimeFilter();
            InitCommands();
            InitTimeFilterChoices();
            LogsPath = "C:\\ProgramData\\Aveva\\Citect SCADA 2018 R2\\Logs";
            LogViewer = new LogView(FilterTime);
            ViewSource = new CollectionViewSource();
            ViewSource.Source = LogViewer;
            ViewSource.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Ascending));
            TreeElems = new ObservableCollection<INodeLog>();
        }

        static MainViewModel _instance;

        string _logsPath="";

        ObservableCollection<INodeLog> _treeElems;

        CollectionViewSource _viewSource;

        TimeFilter _filterTime;

        ICommand _filterSetup, _setTree, _browseFolders,_applyTimeFilter;

        LogView _logView;

        int _hoursBefore;

        List<Tuple<string,int>> _timeFilterChoices = new List<Tuple<string,int>>();


        internal static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainViewModel(true);
                return _instance;
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

        public ICommand ApplyTimeFilter { get => _applyTimeFilter; set => _applyTimeFilter = value; }
        public List<Tuple<string, int>> TimeFilterChoices { get => _timeFilterChoices; set => _timeFilterChoices = value; }
        public int HoursBefore {
            get { 
                return _hoursBefore;
            }
            set {
                if (value == _hoursBefore) return;

                _hoursBefore = value;
                FilterTime.DateStart.Timestamp = DateTime.Now - new TimeSpan(_hoursBefore,0,0);
                FilterTime.DateEnd.Timestamp = DateTime.Now;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FilterTime));            
            } 
        }

        public CollectionViewSource ViewSource { get => _viewSource; set => _viewSource = value; }

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
                    LogFile lgFile = new LogFile(file, cGroup);
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
                LogFile lFile = new LogFile(file,currentGroup);
                currentGroup.LogFiles.Add(lFile);
            }
            TreeElems.Clear();

            TreeElems.Add(clientComponent);
            foreach (var elem in clusterMap)
                TreeElems.Add(elem.Value);
            
        }

        private void InitTimeFilterChoices()
        {
            TimeFilterChoices = new List<Tuple<string, int>>()
            {
                new Tuple<string,int>("Last Hour",1),
                new Tuple<string,int>("Last 6 Hours",6),
                new Tuple<string, int>("Last 24 Hours", 24),
                new Tuple<string, int>("Last 48 Hours", 48),
                new Tuple<string, int>("Last 7 Days", 168),
                new Tuple<string, int>("Last 30 days", 24 * 30)
            };
        }

        private void InitCommands()
        {
            FilterSetup = new DelegateCommand<int>((par) => FilterTime.FilterFromNow(par));
            SetTree = new DelegateCommand(() => ScanLogDirectory(LogsPath));
            BrowseFolders = new DelegateCommand(() => BrowseDirs());
            ApplyTimeFilter = new DelegateCommand(() => ApplyTimeFilterExec());
        }
    
        private void ApplyTimeFilterExec()
        {
            LogViewer.ApplyTimeFilter(FilterTime.DateStart, FilterTime.DateEnd);
            ViewSource.View.Refresh();
        }
    }
}
