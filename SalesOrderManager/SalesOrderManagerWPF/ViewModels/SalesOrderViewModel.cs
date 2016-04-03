using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricorn;

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

        public string LastGroupReference { get; set; }

        public DateTime? LastQuotedOn { get; set; }
        
        public string SalesOrderFileName { get; set; }

        public byte[] PhotoBytes { get; set; }

        public IEnumerable<QuoteDetail> LastQuoteDetails { get; set; }
    }
}
