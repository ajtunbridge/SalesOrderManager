using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesOrderManagerWPF.ViewModels
{
    public class SalesOrderListItemViewModel
    {
        public string OrderNumber { get; set; }
        public string Buyer { get; set; }
        public string DrawingNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string FileName { get; set; }
        public string MailId { get; set; }
        public byte[] CompanyLogoBytes { get; set; }
    }

    public class FakeSalesOrderListItemViewModel
    {
        public string OrderNumber => "4500653265";
        public string Buyer => "Duncan Pegrum";
        public string DrawingNumber => "H522922D";
        public DateTime DeliveryDate => DateTime.Today.AddDays(22);
        public byte[] CompanyLogoBytes => null;
    }
}
