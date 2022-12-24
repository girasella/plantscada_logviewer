using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class TimeInterval : ViewModelBase
    {
        DateTimeWrapper _dateStart;
        DateTimeWrapper _dateEnd;

        public TimeInterval()
        {
            _dateEnd = new DateTimeWrapper(DateTime.Now);
            _dateStart = new DateTimeWrapper(DateTime.Now.AddYears(-1));
        }

        public TimeInterval(DateTimeWrapper dtStart, DateTimeWrapper dtEnd)
        {
            _dateEnd = new DateTimeWrapper(dtEnd.Timestamp);
            _dateStart = new DateTimeWrapper(dtStart.Timestamp);
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

    }
}
