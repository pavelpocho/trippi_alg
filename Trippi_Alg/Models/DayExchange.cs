using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trippi_Alg.Models
{
    public class DayExchange
    {

        public int ID { get; set; }
        public IEnumerable<ExchangeRate> Rates { get; set; }
        public DateTime Date { get; set; }

    }
}
