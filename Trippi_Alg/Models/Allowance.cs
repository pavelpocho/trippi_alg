using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trippi_Alg.Models
{
    public class Allowance
    {

        public int ID { get; set; }
        public int MoneyAmount { get; set; }
        public CurrencyCode Currency { get; set; }

    }
}
