using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class LogGroup : ViewModelBase, INodeLog
    {

        string _name;
        LogType _type;
        List<LogFile> _logFiles;
        public string Name { get => _name; set => _name = value; }
        public LogType Type { get => _type; set => _type = value; }
        internal List<LogFile> LogFiles { get => _logFiles; set => _logFiles = value; }
        public List<INodeLog> Children { get => null; set => throw new NotImplementedException(); }
        public LogGroup(string name, LogType type)
        {
            Name = name;
            Type = type;
            LogFiles = new List<LogFile>();
        }
        public LogGroup()
        {
            Name = "Sys";
            Type = LogType.Sys;
            LogFiles = new List<LogFile>();
        }

    }
}
