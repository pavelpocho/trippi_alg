using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trippi_Alg.Models
{
    public class ExchangeRate
    {

        public int ID { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public double Rate { get; set; }

    }

    public enum CurrencyCode
    {
        EUR = 0,
        USD = 1,
        CZK = 2
    }
}
