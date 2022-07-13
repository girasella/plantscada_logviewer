using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantSCADA_Logviewer
{
    internal class DateTimeWrapper : ViewModelBase
    {
        DateTime _day;

        int _hours, _minutes;

        public DateTime Day
        {
            get
            {
                return _day;
            }

            set
            {
                _day = value;
                OnPropertyChanged();
            }
        }

        public int Hours
        {
            get
            {
                return _hours;
            }
            set
            {
                _hours = value;
                OnPropertyChanged();
            }
        }

        public int Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                _minutes = value;
                OnPropertyChanged();
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return Day.AddHours(Hours).AddMinutes(Minutes);
            }
            set
            {
                Day = new DateTime(value.Year, value.Month, value.Day);
                Hours = value.Hour;
                Minutes = value.Minute;
                OnPropertyChanged();
            }

        }
    }
}
