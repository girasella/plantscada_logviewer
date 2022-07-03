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
        bool _selected;
        INodeLog _parent;
        public string Name { get => _name; set => _name = value; }
        public LogType Type { get => _type; set => _type = value; }
        internal List<LogFile> LogFiles { get => _logFiles; set => _logFiles = value; }
        public List<INodeLog> Children { get => null; set => throw new NotImplementedException(); }

        public LogGroup(string name, LogType type, INodeLog par)
        {
            Name = name;
            Type = type;
            LogFiles = new List<LogFile>();
            Parent = par;
        }
        public LogGroup()
        {
            Name = "Sys";
            Type = LogType.Sys;
            LogFiles = new List<LogFile>();
        }

        public INodeLog Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
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
        public string SourcePath
        {
            get
            {
                INodeLog elem = Parent;
                string retValue = this.Name;
                do
                {
                    
                    retValue = Parent.Name + "." + retValue;
                    elem = elem.Parent;
                }
                while (elem != null);
                return retValue;
            }
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
                    MainViewModel.Instance.LogViewer.RemoveAll(x => x.SourceNode == this);
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
