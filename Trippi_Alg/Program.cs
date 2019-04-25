using System;
using System.Collections.Generic;
using Trippi_Alg.Models;
using Trippi_Alg.BLL;

namespace Trippi_Alg
{
    class Program
    {
        static void Main(string[] args)
        {

            List<Location> locations = GetLocationList();

            

            //Konec první části, máme seznam daySections

            AllowanceManager allowanceManager = new AllowanceManager();
            List <DaySection> daySections = allowanceManager.GetAllowance(locations);

            //Apply allowance calculating algorithm

            //foreach (CountryDuration c in countryDurations)
            //{
            //    Console.WriteLine(c);
            //}

            //Console.WriteLine("---------------------------");

            foreach (DaySection d in daySections)
            {
                Console.WriteLine(d);
            }

            Console.ReadLine();
        }
                
        public static List<Location> GetLocationList()
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
                DepartureTime = Convert.ToInt64(new DateTime(1970, 1, 1, 1, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),

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
                        Rate100 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 50 },
                        Rate66 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 35 },
                        Rate33 = new Allowance { Currency = CurrencyCode.EUR, MoneyAmount = 15 },
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
                        Rate100 = new Allowance { Currency = CurrencyCode.CZK, MoneyAmount = 233 },
                        Rate66 = new Allowance { Currency = CurrencyCode.CZK, MoneyAmount = 120 },
                        Rate33 = new Allowance { Currency = CurrencyCode.CZK, MoneyAmount = 70 },
                    }
                },
                ArrivalDate = Convert.ToInt64(new DateTime(2019, 3, 18, 0, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),
                ArrivalTime = Convert.ToInt64(new DateTime(1970, 1, 1, 20, 0, 0).Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds),

                Food = new LocationFood()
            });

            return list;
        }
    }
}
