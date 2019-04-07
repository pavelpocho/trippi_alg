using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trippi_Alg.Models
{
    public class DayFood
    {
        public int ID { get; set; }
        public bool Breakfast { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }

        public int GetCount()
        {
            int i = 0;
            if (Breakfast) i++;
            if (Lunch) i++;
            if (Dinner) i++;
            return i;
        }
    }
}
