using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data
{
    public class MonthlyData

    {
        public string Year { get; set; }

        public string? Month { get; set; }

        public MonthlyData(string year, string month)
        {
            this.Year = year;
            this.Month = month;
                
        }

    }
}
