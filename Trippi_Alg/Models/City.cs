using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trippi_Alg.Models
{
    public class City
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }

    }
}
