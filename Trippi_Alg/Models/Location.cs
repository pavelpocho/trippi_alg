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
        public DateTime CrossedAt { get; set; }
        public TravelType? InboundTravelType { get; set; }
        public City City { get; set; }
        public DateTime? Arrival { get; set; }
        public DateTime? Departure { get; set; }
        public LocationFood Food { get; set; }

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
