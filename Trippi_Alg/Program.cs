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

            List<CountryDuration> countryDurations = new List<CountryDuration>();

            for (var i = 0; i < locations.Count; i++ ) {

                Location nextCrossing = null;

                for (var j = i + 1; j < locations.Count; j++)
                {
                    if (locations[j].IsCrossing) nextCrossing = locations[j];
                }

                if (i == 0)
                {
                    countryDurations.Add(new CountryDuration { Arrival = locations[i].Departure.Value, Departure = locations[i + 1].CrossedAt, Country = locations[i].City.Country, TimeSpan = locations[i + 1].CrossedAt - locations[i].Departure.Value });
                }
                else if (locations[i].IsCrossing && nextCrossing != null)
                {
                    countryDurations.Add(new CountryDuration { Arrival = locations[i].CrossedAt, Departure = nextCrossing.CrossedAt, Country = locations[i + 1].City.Country, TimeSpan = nextCrossing.CrossedAt - locations[i].CrossedAt });
                }
                else if (locations[i].IsCrossing)
                {
                    countryDurations.Add(new CountryDuration { Arrival = locations[i].CrossedAt, Departure = locations[i + 1].Arrival.Value, Country = locations[i + 1].City.Country, TimeSpan = locations[i + 1].Arrival.Value - locations[i].CrossedAt });
                }
            }

            foreach (CountryDuration c in countryDurations)
            {
                Console.WriteLine(c);
            }

            Console.ReadLine();
        }

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
                Departure = new DateTime(2019, 3, 15, 8, 0, 0)
            });

            list.Add(new Location
            {
                CrossingFrom = new Country { Name = "Czech Republic" },
                CrossingTo = new Country { Name = "Poland" },
                IsCrossing = true,
                CrossedAt = new DateTime(2019, 3, 15, 9, 0, 0)
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
                Arrival = new DateTime(2019, 3, 15, 10, 0, 0),
                Departure = new DateTime(2019, 3, 15, 13, 0, 0)
            });

            list.Add(new Location
            {
                CrossingFrom = new Country { Name = "Poland" },
                CrossingTo = new Country { Name = "Finland" },
                IsCrossing = true,
                CrossedAt = new DateTime(2019, 3, 15, 13, 0, 0)
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
                Arrival = new DateTime(2019, 3, 15, 15, 0, 0),
                Departure = new DateTime(2019, 3, 17, 13, 0, 0)
            });

            list.Add(new Location
            {
                CrossingFrom = new Country { Name = "Finland" },
                CrossingTo = new Country { Name = "Poland" },
                IsCrossing = true,
                CrossedAt = new DateTime(2019, 3, 17, 13, 0, 0)
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
                Arrival = new DateTime(2019, 3, 17, 15, 0, 0),
                Departure = new DateTime(2019, 3, 17, 16, 0, 0)
            });

            list.Add(new Location
            {
                CrossingFrom = new Country { Name = "Poland" },
                CrossingTo = new Country { Name = "Czech Republic" },
                IsCrossing = true,
                CrossedAt = new DateTime(2019, 3, 17, 17, 0, 0)
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
                Arrival = new DateTime(2019, 3, 17, 18, 0, 0)
            });

            return list;
        }
    }
}
