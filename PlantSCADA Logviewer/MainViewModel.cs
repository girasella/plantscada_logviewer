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
using System.Text.RegularExpressions;

namespace PlantSCADA_Logviewer
{
    internal class MainViewModel : ViewModelBase
    {

        /// <summary>
        /// This constructor is used only to see sample data in VS Designer Window.
        /// </summary>
        public MainViewModel()
        {
            TimeRange = new TimeInterval();
            InitCommands();
            InitTimeFilterChoices();
            LogsPath = "C:\\ProgramData\\Aveva\\Citect SCADA 2018 R2\\Logs";
            LogViewer = new LogView(TimeRange);
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
            TimeRange = new TimeInterval();
            InitCommands();
            InitTimeFilterChoices();
            LogsPath = "C:\\ProgramData\\Aveva\\Citect SCADA 2018 R2\\Logs";
            LogViewer = new LogView(TimeRange);
            ViewSource = new CollectionViewSource();
            ViewSource.Source = LogViewer;
            ViewSource.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Ascending));
            TreeElems = new ObservableCollection<INodeLog>();
            filterPredicate = new Predicate<object>(ApplyStringFilter);
        }

        static MainViewModel _instance;

        string _logsPath="", _filterArgument="";

        Predicate<object> filterPredicate;

        private bool[] _modeArray = new bool[] { true, false, false };

        ObservableCollection<INodeLog> _treeElems;

        CollectionViewSource _viewSource;

        TimeInterval _timeRange;

        ICommand  _setTree, _browseFolders,_applyTimeFilter, _applyTextFilter;

        LogView _logView;

        int _hoursBefore;

        List<Tuple<string,int>> _timeRangeChoices = new List<Tuple<string,int>>();

        bool _caseSensitive;

        internal static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainViewModel(true);
                return _instance;
            }
        }
   
        public bool[] ModeArray
        {
            get { return _modeArray; }
        }

        public bool CaseSensitive
        {
            get
            {
                return _caseSensitive;
            }
            set
            {
                _caseSensitive = value;
                OnPropertyChanged();
            }
        }

        public int SelectedMode
        {
            get { return Array.IndexOf(_modeArray, true); }
        }
        public string LogsPath
        {
            get
            {
                return LogsPath1;
            }
            set { 
                LogsPath1 = value; 
                OnPropertyChanged();            
            }
        }

        public string FilterArgument
        {
            get { return _filterArgument; }
            set
            {
                _filterArgument = value;
                OnPropertyChanged();
            }
        }


        public TimeInterval TimeRange
        {
            get { return _timeRange; }

            set { 
                _timeRange = value;
                OnPropertyChanged(); 
            }

        }

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
        public ICommand BrowseFolders { 
            get => _browseFolders; 
            set => _browseFolders = value; 
        }


        public ICommand ApplyTextFilter
        {
            get => _applyTextFilter;
            set => _applyTextFilter = value;
        }
        public ICommand ApplyTimeFilter { get => _applyTimeFilter; set => _applyTimeFilter = value; }

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

        public List<Tuple<string, int>> TimeRangeChoices { get => _timeRangeChoices; set => _timeRangeChoices = value; }
        public int HoursBefore {
            get { 
                return _hoursBefore;
            }
            set {
                if (value == _hoursBefore) return;

                _hoursBefore = value;
                TimeRange.DateStart.Timestamp = DateTime.Now - new TimeSpan(_hoursBefore,0,0);
                TimeRange.DateEnd.Timestamp = DateTime.Now;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TimeRange));            
            } 
        }

        public CollectionViewSource ViewSource { get => _viewSource; set => _viewSource = value; }
        public string LogsPath1 { get => _logsPath; set => _logsPath = value; }

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
                if (file.Extension != ".dat" && file.Extension != ".log" && file.Extension != ".bak")
                    continue;

                string fName = file.Name.ToLower();
                              
                string[] fileNameParts = fName.Split('.');

                string logGroupType = fileNameParts[0];
                
                LogType? lType = LogTypeDictionary.GetLogType(logGroupType);
                if (lType == null)
                    continue;

                if (fileNameParts.Length == 2)
                {
                    LogGroup cGroup = (LogGroup)clientComponent.Children.FirstOrDefault(x => ((LogGroup)x).Type == lType);

                                  

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
        
        internal void TextFilterExec()
        {
            if (string.IsNullOrEmpty(FilterArgument))
                ViewSource.View.Filter = null;
            else
                ViewSource.View.Filter = filterPredicate;

        }
        internal bool ApplyStringFilter(object par) {
            LogEntry lEntry = par as LogEntry;
            string message = CaseSensitive ? lEntry.Message : lEntry.Message.ToLower();
            string filterArg = CaseSensitive ? FilterArgument : FilterArgument.ToLower();
            switch (SelectedMode)
            {
                case 0:
                    return message.Contains(filterArg);
                case 1:
                    return !message.Contains(filterArg);
                case 2:
                    return CaseSensitive ? Regex.IsMatch(lEntry.Message, FilterArgument) : Regex.IsMatch(lEntry.Message, FilterArgument, RegexOptions.IgnoreCase);                  
                default:
                    return true;
            }
        }


        private void InitTimeFilterChoices()
        {
            TimeRangeChoices = new List<Tuple<string, int>>()
            {
                new Tuple<string,int>("Last Hour",1),
                new Tuple<string,int>("Last 6 Hours",6),
                new Tuple<string, int>("Last 24 Hours", 24),
                new Tuple<string, int>("Last 48 Hours", 48),
                new Tuple<string, int>("Last 7 Days", 168),
                new Tuple<string, int>("Last 30 days", 24 * 30),
                new Tuple<string, int>("Last year", 24 * 365)
            };
        }

        private void InitCommands()
        {
            SetTree = new DelegateCommand(() => ScanLogDirectory(LogsPath));
            BrowseFolders = new DelegateCommand(() => BrowseDirs());
            ApplyTimeFilter = new DelegateCommand(() => ApplyTimeFilterExec());
            ApplyTextFilter = new DelegateCommand(() => TextFilterExec());
        }
    
        private void ApplyTimeFilterExec()
        {
            LogViewer.ApplyTimeFilter(TimeRange.DateStart.Timestamp, TimeRange.DateEnd.Timestamp);
            ViewSource.View.Refresh();
        }
    }
}
