using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class Items
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public string orderNumber { get; set; }
        public string orderItemNumber { get; set; }
        public SKUContent sku { get; set; }
        public string batchNumber { get; set; }
        public string grossWeight { get; set; }
        public string netWeight { get; set; }
        public string receivingQty { get; set; }
        public string actualQty { get; set; }
        public string sortedQty { get; set; }
        public string shortageQty { get; set; }
        public string excessQty { get; set; }
        public string damagedQty { get; set; }
        public string weight { get; set; }
        public string volume { get; set; }
    }

    public class ItemsD
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        //public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public SKUContent sku { get; set; }
        public string qty { get; set; }
        public string unitPrice { get; set; }
        public string totalPrice { get; set; }
    }
}
