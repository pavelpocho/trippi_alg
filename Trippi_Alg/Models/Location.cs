using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trippi_Alg.Models
{
    public class Location
    {

        public int ID { get; set; }
        public bool IsCrossing { get; set; }
        public Country CrossingFrom { get; set; }
        public Country CrossingTo { get; set; }
        public long? CrossedAt { get; set; } //Number of milliseconds since departure from last point
        public bool CrossedBorder { get; set; }
        public TravelType? InboundTravelType { get; set; }
        public City City { get; set; }

        //Milliseconds to midnight of the correct day
        public long? ArrivalDate { get; set; }
        //Milliseconds to correct time since midnight
        public long? ArrivalTime { get; set; }

        //Milliseconds to midnight of the correct day
        public long? DepartureDate { get; set; }
        //Milliseconds to correct time since midnight
        public long? DepartureTime { get; set; }
        public LocationFood Food { get; set; }
        public int TripId { get; set; }
        public int Position { get; set; }
        public bool Deleted { get; set; }

    }

    public enum TravelType
    {
        COMPANY_CAR = 0,
        OWN_CAR = 1,
        SLOW_TRAIN = 2,
        FAST_TRAIN = 3,
        BUS = 4,
        PLANE = 5
    }
}
