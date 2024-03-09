using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class SupplierType
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        //public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

    }
}
