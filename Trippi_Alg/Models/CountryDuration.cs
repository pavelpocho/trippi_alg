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
            Arrival = DateTime.MinValue;
            Departure = DateTime.MinValue;
            Country = new Country();
            TimeSpan = Arrival - Departure;
        }

        public Country Country { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public DateTime Arrival { get; set; }
        public DateTime Departure { get; set; }

        public override string ToString()
        {
            return String.Format("Country: {0}, Arrival: {1}, Departure: {2}, TimeSpan: {3}", Country.Name, Arrival.ToString(), Departure.ToString(), TimeSpan.Days);
        }

    }
}
