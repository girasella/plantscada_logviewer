using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PlantSCADA_Logviewer
{
    internal class LogFile : ViewModelBase
    {
        FileInfo _file;
        public LogFile(string fname)
        {
            FileName = fname; 
            _file = new FileInfo(fname);
        }
        
        string _fileName;
        DateTime _start, _end;

        public string FileName { get => _fileName; set => _fileName = value; }
        public DateTime Start { get => _start; set => _start = value; }
        public DateTime End { get => _end; set => _end = value; }
    }
}
