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
        List<LogEntry> _logEntries;
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
                var entries = file.LogEntries;

                logEntries.AddRange(entries);

            }
            return logEntries;
        }
        public string SourcePath
        {
            get
            {
                INodeLog elem = this.Parent;
                string retValue = this.Name;
                do
                {
                    
                    retValue = elem.Name + "." + retValue;
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
                    MainViewModel.Instance.LogViewer.AddGroup(this);
                    
                }
                else
                {
                    MainViewModel.Instance.LogViewer.RemoveGroup(this);
                }

                MainViewModel.Instance.ViewSource.View.Refresh();

               _selected = value;

                OnPropertyChanged();

                Parent.UpdateSelectedProperty();
            }
        }

        internal List<LogEntry> LogEntries {
            get
            {
                if (_logEntries == null)
                {
                    _logEntries = new List<LogEntry>();
                    foreach (var file in LogFiles)
                    {
                        _logEntries.AddRange(file.LogEntries);
                    }                    
                }
                return _logEntries;
            }
        }
        void INodeLog.AddChild(INodeLog child)
        {
            Children.Add(child);
        }

        void INodeLog.RemoveChild(INodeLog child)
        {
            Children.Remove(child);
        }
        public void UpdateSelectedProperty()
        {
            OnPropertyChanged(nameof(Selected));

        }
    }
}
