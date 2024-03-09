using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class Supplier
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public SupplierType supplierType { get; set; }
        public string plantNumber { get; set; }
        public string name { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string pin { get; set; }
        public string countryCode { get; set; }
        public string regionCode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }

    }
}
