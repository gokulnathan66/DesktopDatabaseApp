using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data
{
    public class GenericReportData

    {
        public string Year { get; set; }

        public string? SupplierName { get; set; }

        public GenericReportData(string year, string suppler)
        {
            this.Year = year;
            this.SupplierName = suppler;
        
        }

    }
}
