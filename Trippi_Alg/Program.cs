using System;
using System.Collections.Generic;
using Trippi_Alg.Models;

namespace Trippi_Alg
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Location> locations = GetLocationList();

            long tripStartDate = locations.ToArray()[0].DepartureDate.Value;

            List<CountryDuration> countryDurations = new List<CountryDuration>();

            for (var i = 0; i < locations.Count; i++ ) {

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
                    countryDurations.Add(new CountryDuration {
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
                    countryDurations.Add(new CountryDuration {
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
                    countryDurations.Add(new CountryDuration {
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

            //Konec první části, máme seznam daySections



            foreach (CountryDuration c in countryDurations)
            {
                Console.WriteLine(c);
            }

            Console.WriteLine("---------------------------");

            foreach (DaySection d in daySections)
            {
                Console.WriteLine(d);
            }

            Console.ReadLine();
        }

        private int hoursForThird = 5;
        private int hoursForTwoThirds = 12;
        private int hoursForFull = 18;

        private DaySectionAllowance GetRate(Country country, long hours, int foods)
        {
            if (hours < hoursForThird)
            {
                return null;
            }
            else if (hours >= hoursForThird && hours < hoursForTwoThirds)
            {
                DaySectionAllowance d = new DaySectionAllowance(country.Rate33, (int) hours, foods);
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

        private List<DaySection> GetAllowance(List<DaySection> daySections)
        {
            List<DaySection> czTwoDay = GetAllowanceCZTwoDay(daySections);
            List<DaySection> cz = GetAllowanceCZ(czTwoDay);
            List<DaySection> final = GetAllowanceGeneral(cz);

            return final;
        }

        private List<DaySection> GetAllowanceCZTwoDay(List<DaySection> daySections)
        {
            if (!IsAllowanceCZTwoDay(daySections)) return daySections;

            var left = GetRate(daySections[0].Country, daySections[0].Duration, daySections[0].Food.GetCount()).MoneyAmount + GetRate(daySections[1].Country, daySections[1].Duration, daySections[1].Food.GetCount()).MoneyAmount;
            var right = GetRate(daySections[0].Country, daySections[0].Duration + daySections[1].Duration, daySections[0].Food.GetCount() + daySections[1].Food.GetCount()).MoneyAmount;

            daySections[0].Allowance = left > right ? GetRate(daySections[0].Country, daySections[0].Duration, daySections[0].Food.GetCount()) : null;
            daySections[1].Allowance = left > right ? GetRate(daySections[1].Country, daySections[1].Duration, daySections[0].Food.GetCount()) : GetRate(daySections[0].Country, daySections[0].Duration + daySections[1].Duration, daySections[0].Food.GetCount() + daySections[1].Food.GetCount());

            return daySections;
        }

        private bool IsAllowanceCZTwoDay(List<DaySection> daySections)
        {
            return daySections.Count == 2 &&
                   daySections[0].Country.Name == "Czech Republic" &&
                   daySections[1].Country.Name == "Czech Republic" &&
                   daySections[0].Duration + daySections[1].Duration <= 24;
        }

        private List<DaySection> GetAllowanceCZ(List<DaySection> daySections)
        {

        }

        private List<DaySection> GetAllowanceGeneral(List<DaySection> daySections)
        {

        }

        //1. Řešit CZ
        //2. Pokud má trip 2 dny tak se všechno spočítá dvakrát a vyhraje větší
        //!. Konfiguračně zapnutelné a vypnutelné

        //1. Řešit CZ
        //2. Vyřešit CZ

        //1. Neřešit CZ
        //2. Vytvořit celkový priority rating pro každou zemi podle celkové doby
        //3. Pro každý kalendářní den připočíst všechny hodiny, které jsou větší než 1, ke státu s nejlepším priority ratingem v tom dni

        static List<Location> GetLocationList()
        {
            List<Location> list = new List<Location>();

            list.Add(new Location
            {
                City = new City
                {
                    Name = "Ostrava",
                    Country = new Country
                    {
                        Name = "Czech Republic",
                        Rate100 = new Allowance { Currency = CurrencyCode.CZK, MoneyAmount = 233 },
                        Rate66 = new Allowance { Currency = CurrencyCode.CZK, MoneyAmount = 120 },
                        Rate33 = new Allowance { Currency = CurrencyCode.CZK, MoneyAmount = 70 },
                    }
                },

                DepartureDate = Convert.ToInt64(new DateTime(2019, 3, 15, 0, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
                DepartureTime = Convert.ToInt64(new DateTime(1970, 1, 1, 8, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),

                Food = new LocationFood()

            });

            list.Add(new Location
            {
                CrossingFrom = new Country { Name = "Czech Republic" },
                CrossingTo = new Country { Name = "Poland" },
                CrossedBorder = true,
                CrossedAt = Convert.ToInt64(new DateTime(1970, 1, 1, 1, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
            });

            list.Add(new Location
            {
                City = new City
                {
                    Name = "Krakow",
                    Country = new Country
                    {
                        Name = "Poland",
                        Rate100 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 50 },
                        Rate66 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 35 },
                        Rate33 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 15 },
                    }
                },
                ArrivalDate = Convert.ToInt64(new DateTime(2019, 3, 15, 0, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
                ArrivalTime = Convert.ToInt64(new DateTime(1970, 1, 1, 10, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
                DepartureDate = Convert.ToInt64(new DateTime(2019, 3, 15, 0, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
                DepartureTime = Convert.ToInt64(new DateTime(1970, 1, 1, 11, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),

                Food = new LocationFood()
            });

            list.Add(new Location
            {
                CrossingFrom = new Country { Name = "Poland" },
                CrossingTo = new Country { Name = "Finland" },
                CrossedBorder = true,
                CrossedAt = 0,
            });

            list.Add(new Location
            {
                City = new City
                {
                    Name = "Helsinki",
                    Country = new Country
                    {
                        Name = "Finland",
                        Rate100 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 80 },
                        Rate66 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 45 },
                        Rate33 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 25 },
                    }
                },
                ArrivalDate = Convert.ToInt64(new DateTime(2019, 3, 15, 0, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
                ArrivalTime = Convert.ToInt64(new DateTime(1970, 1, 1, 13, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
                DepartureDate = Convert.ToInt64(new DateTime(2019, 3, 17, 0, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
                DepartureTime = Convert.ToInt64(new DateTime(1970, 1, 1, 10, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),

                Food = new LocationFood { FirstDay = new DayFood { Dinner = true }, MiddleDays = new List<DayFood> { new DayFood { Breakfast = true, Lunch = true, Dinner = true } }, LastDay = new DayFood { Breakfast = true } }
            });

            list.Add(new Location
            {
                CrossingFrom = new Country { Name = "Finland" },
                CrossingTo = new Country { Name = "Poland" },
                CrossedBorder = true,
                CrossedAt = 0
            });

            list.Add(new Location
            {
                City = new City
                {
                    Name = "Krakow",
                    Country = new Country
                    {
                        Name = "Poland",
                        Rate100 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 80 },
                        Rate66 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 45 },
                        Rate33 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 25 },
                    }
                },
                ArrivalDate = Convert.ToInt64(new DateTime(2019, 3, 17, 0, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
                ArrivalTime = Convert.ToInt64(new DateTime(1970, 1, 1, 12, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
                DepartureDate = Convert.ToInt64(new DateTime(2019, 3, 18, 0, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
                DepartureTime = Convert.ToInt64(new DateTime(1970, 1, 1, 13, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),

                Food = new LocationFood { FirstDay = new DayFood { Dinner = true }, LastDay = new DayFood { Breakfast = true }, MiddleDays = new List<DayFood>() }
            });

            list.Add(new Location
            {
                CrossingFrom = new Country { Name = "Poland" },
                CrossingTo = new Country { Name = "Czech Republic" },
                CrossedBorder = true,
                CrossedAt = Convert.ToInt64(new DateTime(1970, 1, 1, 1, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds)
            });

            list.Add(new Location
            {
                City = new City
                {
                    Name = "Ostrava",
                    Country = new Country
                    {
                        Name = "Czech Republic",
                        Rate100 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 80 },
                        Rate66 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 45 },
                        Rate33 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 25 },
                    }
                },
                ArrivalDate = Convert.ToInt64(new DateTime(2019, 3, 18, 0, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
                ArrivalTime = Convert.ToInt64(new DateTime(1970, 1, 1, 15, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),

                Food = new LocationFood()
            });

            return list;
        }
    }
}
