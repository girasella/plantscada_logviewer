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


        internal DateTimeWrapper(DateTime dt)
        {
            Timestamp = dt;
        }

        public DateTime SelectedDay
        {
            get
            {
                return _day;
            }

            set
            {
                _day = value;
                OnPropertyChanged(nameof(SelectedDay));
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
                return SelectedDay.AddHours(Hours).AddMinutes(Minutes);
            }
            set
            {
                SelectedDay = new DateTime(value.Year, value.Month, value.Day);
                Hours = value.Hour;
                Minutes = value.Minute;
                OnPropertyChanged();
            }

        }
    }
}
