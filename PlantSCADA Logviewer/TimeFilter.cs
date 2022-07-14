using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class TimeFilter : ViewModelBase
    {
        DateTimeWrapper _dateStart;
        DateTimeWrapper _dateEnd;

        public TimeFilter()
        {
            _dateEnd = new DateTimeWrapper(DateTime.Now);
            _dateStart = new DateTimeWrapper(DateTime.Now.AddDays(-7));
        }

        public TimeFilter(DateTimeWrapper dtStart, DateTimeWrapper dtEnd)
        {
            _dateEnd = new DateTimeWrapper(dtStart.Timestamp);
            _dateStart = new DateTimeWrapper(dtEnd.Timestamp);
        }

        public DateTimeWrapper DateStart
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

        public DateTimeWrapper DateEnd
        {
            get { return _dateEnd; }

            set {
                _dateEnd = value;
                OnPropertyChanged(); 
            }
        }


        public void FilterSetup(DateTime start, DateTime end)
        {
            DateStart.Timestamp = start;
            DateEnd.Timestamp = end;
        }
        public void FilterFromNow(int nHours)
        {
            DateStart.Timestamp = DateTime.Now.Subtract(new TimeSpan(nHours,0,0));
            DateEnd.Timestamp = DateTime.Now;
        }

    }
}
