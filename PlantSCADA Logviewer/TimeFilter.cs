using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class TimeFilter : ViewModelBase
    {
        DateTime _dateStart;
        DateTime _dateEnd;

        public TimeFilter()
        {
            _dateStart = DateTime.MinValue;
            _dateEnd = DateTime.MaxValue;
        }

        public TimeFilter(DateTime dateStart, DateTime dateEnd)
        {
            DateStart = dateStart;
            DateEnd = dateEnd;
        }

        public DateTime DateStart
        {
            get
            {
                return _dateStart;
            }
            set
            {
                _dateStart = value;
                OnPropertyChanged();
            }
        }

        public DateTime DateEnd
        {
            get { return _dateEnd; }

            set {
                _dateEnd = value;
                OnPropertyChanged(); 
            }
        }


        public void FilterSetup(DateTime start, DateTime end)
        {
            DateStart = start;
            DateEnd = end;
        }
        public void FilterFromNow(int nHours)
        {
            DateStart = DateTime.Now.Subtract(new TimeSpan(nHours,0,0));
            DateEnd = DateTime.Now;
        }

    }
}
