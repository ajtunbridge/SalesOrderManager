#region Using directives

using System;

#endregion

namespace SalesOrderParser
{
    public sealed class SalesOrderDetail
    {
        public string Buyer { get; set; }
        public string OrderNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DrawingNumber { get; set; }
        public string FileName { get; set; }

        /// <summary>
        /// Returns the order number and due date as follows: 450002435 (23/04/2016)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DeliveryDate == DateTime.MinValue ? $"{OrderNumber} (N/A)" : $"{OrderNumber} ({DeliveryDate.ToShortDateString()})";
        }
    }
}