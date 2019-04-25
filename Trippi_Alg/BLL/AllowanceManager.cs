using System;
using System.Collections.Generic;
using System.Text;
using Trippi_Alg.Models;

namespace Trippi_Alg.BLL
{
    class AllowanceManager : IAllowanceManager
    {
        private List<DaySection> GetDaySections (List<Location> locations)
        {
            long tripStartDate = locations.ToArray()[0].DepartureDate.Value;

            List<CountryDuration> countryDurations = new List<CountryDuration>();

            for (var i = 0; i < locations.Count; i++)
            {

                Location nextCrossing = null;

                var j = i + 1;

                for (; j < locations.Count; j++)
                {
                    if (locations[j].CrossedBorder)
                    {
                        nextCrossing = locations[j];
                        break;
                    }
                }

                if (i == 0)
                {
                    countryDurations.Add(new CountryDuration
                    {
                        ArrivalDate = locations[i].DepartureDate.Value,
                        ArrivalTime = locations[i].DepartureTime.Value,
                        DepartureDate = Convert.ToInt64((locations[i].DepartureDate.Value + locations[i].DepartureTime.Value + locations[i + 1].CrossedAt) / (24 * 60 * 60000) * (24 * 60 * 60000)),
                        DepartureTime = Convert.ToInt64((locations[i].DepartureDate.Value + locations[i].DepartureTime.Value + locations[i + 1].CrossedAt) % (24 * 60 * 60000)),
                        Country = locations[i].City.Country,
                        MillisEachDay = new long[0],
                        Food = locations[i].Food
                    });
                }
                else if (locations[i].CrossedBorder && nextCrossing != null)
                {
                    countryDurations.Add(new CountryDuration
                    {
                        ArrivalDate = Convert.ToInt64((locations[i - 1].DepartureDate.Value + locations[i - 1].DepartureTime.Value + locations[i].CrossedAt) / (24 * 60 * 60000) * (24 * 60 * 60000)),
                        ArrivalTime = Convert.ToInt64((locations[i - 1].DepartureDate.Value + locations[i - 1].DepartureTime.Value + locations[i].CrossedAt) % (24 * 60 * 60000)),
                        DepartureDate = Convert.ToInt64((locations[j - 1].DepartureDate.Value + locations[j - 1].DepartureTime.Value + locations[j].CrossedAt) / (24 * 60 * 60000) * (24 * 60 * 60000)),
                        DepartureTime = Convert.ToInt64((locations[j - 1].DepartureDate.Value + locations[j - 1].DepartureTime.Value + locations[j].CrossedAt) % (24 * 60 * 60000)),
                        Country = locations[i + 1].City.Country,
                        MillisEachDay = new long[0],
                        Food = locations[j - 1].Food
                    });
                }
                else if (locations[i].CrossedBorder)
                {
                    countryDurations.Add(new CountryDuration
                    {
                        ArrivalDate = Convert.ToInt64((locations[i - 1].DepartureDate.Value + locations[i - 1].DepartureTime.Value + locations[i].CrossedAt) / (24 * 60 * 60000) * (24 * 60 * 60000)),
                        ArrivalTime = Convert.ToInt64((locations[i - 1].DepartureDate.Value + locations[i - 1].DepartureTime.Value + locations[i].CrossedAt) % (24 * 60 * 60000)),
                        DepartureDate = locations[i + 1].ArrivalDate.Value,
                        DepartureTime = locations[i + 1].ArrivalTime.Value,
                        Country = locations[i + 1].City.Country,
                        MillisEachDay = new long[0],
                        Food = locations[i + 1].Food
                    });
                }
            }

            List<DaySection> daySections = new List<DaySection>();

            foreach (CountryDuration c in countryDurations)
            {
                daySections.AddRange(c.GetDaySections(tripStartDate));
            }
            return daySections;
        }

        private int hoursForThird = 5 * 3600000;
        private int hoursForTwoThirds = 12 * 3600000;
        private int hoursForFull = 18 * 3600000;

        private DaySectionAllowance GetRate(Country country, long hours, int foods)
        {
            if (hours < hoursForThird)
            {
                return null;
            }
            else if (hours >= hoursForThird && hours < hoursForTwoThirds)
            {
                DaySectionAllowance d = new DaySectionAllowance(country.Rate33, (int)hours, foods);
                d.MoneyAmount -= (foods * 0.7) * d.MoneyAmount;
                return d;
            }
            else if (hours >= hoursForTwoThirds && hours < hoursForFull)
            {
                DaySectionAllowance d = new DaySectionAllowance(country.Rate66, (int)hours, foods);
                d.MoneyAmount -= (foods * 0.35) * d.MoneyAmount;
                return d;
            }
            else
            {
                DaySectionAllowance d = new DaySectionAllowance(country.Rate100, (int)hours, foods);
                d.MoneyAmount -= (foods * 0.25) * d.MoneyAmount;
                return d;
            }
        }

        public List<DaySection> GetAllowance(List<Location> locations)
        {
            List<DaySection> daySections = GetDaySections(locations);
            List<DaySection> czTwoDay = GetAllowanceCZTwoDay(daySections);
            List<DaySection> cz = GetAllowanceCZ(czTwoDay);
            List<DaySection> final = GetAllowanceGeneral(cz);

            return final;
        }

        public List<DaySection> GetAllowanceCZTwoDay(List<DaySection> daySections)
        {
            if (!IsAllowanceCZTwoDay(daySections)) return daySections;

            var left = GetRate(daySections[0].Country, daySections[0].Duration, daySections[0].Food.GetCount()).MoneyAmount + GetRate(daySections[1].Country, daySections[1].Duration, daySections[1].Food.GetCount()).MoneyAmount;
            var right = GetRate(daySections[0].Country, daySections[0].Duration + daySections[1].Duration, daySections[0].Food.GetCount() + daySections[1].Food.GetCount()).MoneyAmount;

            daySections[0].Allowance = left > right ? GetRate(daySections[0].Country, daySections[0].Duration, daySections[0].Food.GetCount()) : null;
            daySections[1].Allowance = left > right ? GetRate(daySections[1].Country, daySections[1].Duration, daySections[0].Food.GetCount()) : GetRate(daySections[0].Country, daySections[0].Duration + daySections[1].Duration, daySections[0].Food.GetCount() + daySections[1].Food.GetCount());
            Console.WriteLine("twodays");
            return daySections;
        }

        public bool IsAllowanceCZTwoDay(List<DaySection> daySections)
        {
            return daySections.Count == 2 &&
                   daySections[0].Country.Name == "Czech Republic" &&
                   daySections[1].Country.Name == "Czech Republic" &&
                   daySections[0].Duration + daySections[1].Duration <= 24;
        }

        public List<DaySection> GetAllowanceCZ(List<DaySection> daySections)
        {
            if (IsAllowanceCZTwoDay(daySections)) return daySections;

            foreach (DaySection d in daySections)
            {
                if (d.Country.Name == "Czech Republic")
                {
                    int food = d.Food != null ? d.Food.GetCount() : 0;
                    d.Allowance = GetRate(d.Country, d.Duration, food);
                }
               
            }

            return daySections;
        }

        public List<List<DaySection>> SortByDayIndex(List<DaySection> daySections)
        {
            List<List<DaySection>> byDayIndex = new List<List<DaySection>>();
            int lastDayIndex = daySections[daySections.Count - 1].DayIndex;
            for (int i = 0; i <= lastDayIndex; i++)
            {
                byDayIndex.Add(daySections.FindAll(item => item.DayIndex == i));
            }
            return byDayIndex;
        }


        public List<DaySection> GetAllowanceGeneral(List<DaySection> daySections)
        {
            List<List<DaySection>> sortByDayIndex = SortByDayIndex(daySections);
            


            for(var i = 0; i < sortByDayIndex.Count; i++)
            {
                int totalFood = 0;
                long totalDuration = 0;
                int longestIndex = 0;
                for (var j = 0; j < sortByDayIndex[i].Count; j++)
                {
                    if (sortByDayIndex[i][j].Country.Name != "Czech Republic")
                    {
                        totalDuration += sortByDayIndex[i][j].Duration;
                        totalFood += sortByDayIndex[i][j].Food != null ? sortByDayIndex[i][j].Food.GetCount() : 0;
                        if (sortByDayIndex[i][j].Duration > sortByDayIndex[i][longestIndex].Duration)
                        {
                            longestIndex = j;
                        }
                    }
                }
                sortByDayIndex[i][longestIndex].Allowance = GetRate(sortByDayIndex[i][longestIndex].Country, totalDuration, totalFood);
            }

            return daySections;
        }


        //1. Řešit CZ
        //2. Pokud má trip 2 dny tak se všechno spočítá dvakrát a vyhraje větší
        //!. Konfiguračně zapnutelné a vypnutelné

        //1. Řešit CZ
        //2. Vyřešit CZ

        //1. Neřešit CZ
        //2. Vytvořit celkový priority rating pro každou zemi podle celkové doby
        //3. Pro každý kalendářní den připočíst všechny hodiny, které jsou větší než 1, ke státu s nejlepším priority ratingem v tom dni

    }
}
