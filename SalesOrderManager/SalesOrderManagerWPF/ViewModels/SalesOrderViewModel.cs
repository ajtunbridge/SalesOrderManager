using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrderManagerWPF.ViewModels
{
    public class SalesOrderViewModel
    {
        public string Customer { get; set; }

        public string DrawingNumber { get; set; }

        public string Name { get; set; }

        public string OrderNumber { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string LastWorksOrderNumber { get; set; }

        public string LastQuoteNumber { get; set; }

        public DateTime? LastQuotedOn { get; set; }
        
        public string SalesOrderFileName { get; set; }

        public string DrawingFileName { get; set; }

        public byte[] PhotoBytes { get; set; }
    }
}
