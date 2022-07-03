using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlantSCADA_Logviewer
{
    internal class LogGroup : ViewModelBase, INodeLog
    {

        string _name;
        LogType _type;
        List<LogFile> _logFiles;
        ICommand _addToNewView;
        bool _selected;
        public string Name { get => _name; set => _name = value; }
        public LogType Type { get => _type; set => _type = value; }
        internal List<LogFile> LogFiles { get => _logFiles; set => _logFiles = value; }
        public List<INodeLog> Children { get => null; set => throw new NotImplementedException(); }
        public ICommand AddToView {
            get => _addToNewView;
            set => _addToNewView = value;
        }

        public LogGroup(string name, LogType type)
        {
            Name = name;
            Type = type;
            LogFiles = new List<LogFile>();
            AddToView = new DelegateCommand<ObservableCollection<LogView>>((lst) => AddToLogViewExec(lst));
        }
        public LogGroup()
        {
            Name = "Sys";
            Type = LogType.Sys;
            LogFiles = new List<LogFile>();
            AddToView = new DelegateCommand<ObservableCollection<LogView>>((lst) => AddToLogViewExec(lst));
        }

        public List<LogEntry> Load(DateTime start, DateTime end)
        {
            List<LogEntry> logEntries = new List<LogEntry>();

            foreach (var file in LogFiles)
            {
                var entries = file.Load(start, end);

                logEntries.AddRange(entries);

            }
            return logEntries;
        }

        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (value)
                {
                    MainViewModel.Instance.LogViewer.Merge(this.Load(MainViewModel.Instance.FilterTime.DateStart, MainViewModel.Instance.FilterTime.DateEnd));
                }
                else
                {
                    MainViewModel.Instance.LogViewer.Clear();
                }
                
                _selected = value;

                OnPropertyChanged();
            }
        }

        void AddToLogViewExec(ObservableCollection<LogView> lViews)
        {
            LogView logView = new LogView(this.Name, this.Load(new DateTime(0),DateTime.Now));

            lViews.Add(logView);

        }

    }
}
