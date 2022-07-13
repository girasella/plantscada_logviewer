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
            _dateEnd = new DateTimeWrapper();
            _dateStart = new DateTimeWrapper();

            _dateStart.Timestamp = DateTime.MinValue;
            _dateEnd.Timestamp = DateTime.MaxValue;
        }

        public TimeFilter(DateTime dateStart, DateTime dateEnd)
        {
            _dateEnd = new DateTimeWrapper();
            _dateStart = new DateTimeWrapper();
            DateStart.Timestamp = dateStart;
            DateEnd.Timestamp = dateEnd;
        }
        public TimeFilter(DateTimeWrapper dateStart, DateTimeWrapper dateEnd)
        {
            _dateEnd = new DateTimeWrapper();
            _dateStart = new DateTimeWrapper();
            DateStart = dateStart;
            DateEnd = dateEnd;
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
