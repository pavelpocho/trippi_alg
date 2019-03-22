using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trippi_Alg.Models
{
    public class Trip
    {

        public int ID { get; set; }
        public string Title { get; set; }
        public string Purpose { get;set; }
        public string Project { get; set; }
        public string Task { get; set; }
        public string Comment { get; set; }
        public bool Deleted { get; set; }
        public bool Exported { get; set; }
        public DateTime? StartDate { get; set; }
        public IList<ExchangeRate> ExchangeRates { get; set; }
        public IList<Location> Locations { get; set; }
        public int UserID { get; set; }

    }
}
