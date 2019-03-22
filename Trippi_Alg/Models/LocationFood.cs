using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trippi_Alg.Models
{
    public class LocationFood
    {
        public int ID { get; set; }
        public DayFood FirstDay { get; set; }
        public DayFood MiddleDays { get; set; }
        public DayFood LastDay { get; set; }
        public DayFood OnlyDay { get; set; }
    }
}
