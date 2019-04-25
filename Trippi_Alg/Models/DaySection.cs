using System;
using System.Collections.Generic;
using System.Text;

namespace Trippi_Alg.Models
{
    class DaySection
    {
        public Country Country { get; set; }
        public int DayIndex { get; set; }
        public long Duration { get; set; }
        public DayFood Food { get; set; }
        public DaySectionAllowance Allowance { get; set; }

        public override string ToString()
        {

            StringBuilder s = new StringBuilder();

            s.Append("Country: ");
            s.Append(Country.Name);
            s.Append(" ;; ");
            s.Append("DayIndex: ");
            s.Append(DayIndex);
            s.Append(" ;; ");
            s.Append("Duration: ");
            s.Append(Duration / 60000 / 60);
            s.Append(" ;; ");
            if (Food != null)
            {
                s.Append(Food.Breakfast ? "B " : "");
                s.Append(Food.Lunch ? "L " : "");
                s.Append(Food.Dinner ? "D " : "");
            }
            s.Append(" ;; ");
           
            if (Allowance != null)
            {
                s.Append("Allowance:");
                s.Append(Allowance.MoneyAmount);
                s.Append(" ");
                s.Append(Allowance.Currency);
            }
               
            return s.ToString();
        }
    }
}
