using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class ProductCategory
    {
        public int createdBy { get; set; }
        public DateTime createdAt { get; set; }
        //public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string recordStatus { get; set; }
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        //public string parentId { get; set; }
    }
}
