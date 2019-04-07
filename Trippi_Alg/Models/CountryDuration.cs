using System;
using System.Collections.Generic;
using System.Text;
using Trippi_Alg.Models;

namespace Trippi_Alg.Models
{
    class CountryDuration
    {

        public CountryDuration()
        {
            ArrivalDate = 0;
            ArrivalTime = 0;
            DepartureDate = 0;
            DepartureTime = 0;
            Country = new Country();
            MillisEachDay = new long[0];
        }

        public Country Country { get; set; }
        public long ArrivalDate { get; set; }
        public long ArrivalTime { get; set; }
        public long DepartureDate { get; set; }
        public long DepartureTime { get; set; }
        public long[] MillisEachDay { get; set; }
        public LocationFood Food { get; set; }

        public List<DaySection> GetDaySections(long tripStartDate)
        {
            List<DaySection> ds = new List<DaySection>();

            DateTime EpochBase = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
            DateTime Arrival = EpochBase.AddMilliseconds(ArrivalDate + ArrivalTime);
            DateTime Departure = EpochBase.AddMilliseconds(DepartureDate + DepartureTime);

            DayFood[] pointFoods;

            if (ArrivalDate != DepartureDate)
            {
                long TotalMillisSpent = DepartureDate + DepartureTime - ArrivalDate - ArrivalTime;
                long FirstDay = (24 * 60 * 60000) - ArrivalTime;
                long LastDay = DepartureTime;
                long DayCount = (TotalMillisSpent - FirstDay - LastDay) / (24 * 60 * 60000);

                MillisEachDay = new long[DayCount + 2];
                pointFoods = new DayFood[DayCount + 2];
                MillisEachDay[0] = FirstDay;
                pointFoods[0] = Food.FirstDay;
                MillisEachDay[MillisEachDay.Length - 1] = LastDay;
                pointFoods[pointFoods.Length - 1] = Food.LastDay;
                for (int i = 1; i < MillisEachDay.Length - 1; i++)
                {
                    MillisEachDay[i] = (24 * 60 * 60000);
                    pointFoods[i] = Food.MiddleDays[i - 1];
                }
            }
            else
            {
                MillisEachDay = new long[1];
                pointFoods = new DayFood[1];
                MillisEachDay[0] = DepartureTime - ArrivalTime;
                pointFoods[0] = Food.OnlyDay;
            }

            for (var i = 0; i < MillisEachDay.Length; i++)
            {
                ds.Add(new DaySection
                {
                    Duration = MillisEachDay[i],
                    DayIndex = (int)((ArrivalDate + ArrivalTime - tripStartDate) / 60000 / 60 / 24 + i),
                    Food = pointFoods[i],
                    Country = Country
                });
            }

            return ds;

        }

        public override string ToString()
        {
            DateTime EpochBase = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
            DateTime Arrival = EpochBase.AddMilliseconds(ArrivalDate + ArrivalTime);
            DateTime Departure = EpochBase.AddMilliseconds(DepartureDate + DepartureTime);

            if (ArrivalDate != DepartureDate)
            {
                long TotalMillisSpent = DepartureDate + DepartureTime - ArrivalDate - ArrivalTime;
                long FirstDay = (24 * 60 * 60000) - ArrivalTime;
                long LastDay = DepartureTime;
                long DayCount = (TotalMillisSpent - FirstDay - LastDay) / (24 * 60 * 60000);

                MillisEachDay = new long[DayCount + 2];
                MillisEachDay[0] = FirstDay;
                MillisEachDay[MillisEachDay.Length - 1] = LastDay;
                for (int i = 1; i < MillisEachDay.Length - 1; i++)
                {
                    MillisEachDay[i] = (24 * 60 * 60000);
                }
            }
            else
            {
                MillisEachDay = new long[1];
                MillisEachDay[0] = DepartureTime - ArrivalTime;
            }

            StringBuilder s = new StringBuilder();

            for (var i = 0; i < MillisEachDay.Length; i++)
            {
                s.Append((MillisEachDay[i] / 60000 / 60).ToString());
                s.Append(" ");
            }

            return String.Format("Country: {0}, Arrival: {1}, Departure: {2}, Hours in days: {3}", Country.Name, Arrival.ToString(), Departure.ToString(), s.ToString());
        }
    }
}
