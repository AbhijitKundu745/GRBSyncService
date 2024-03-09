using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class SKU
    {
        public List<SKUContent> content { get; set; }
        public Pageable pageable { get; set; }
        public string totalPages { get; set; }
        public string totalElements { get; set; }
        public bool last { get; set; }
        public string size { get; set; }
        public string number { get; set; }
        public Sort sort { get; set; }
        public bool first { get; set; }
        public string numberOfElements { get; set; }
        public bool empty { get; set; }
    }

    public class SKUContent
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public SPU spu { get; set; } //
        public string code { get; set; }
        public string name { get; set; }
        public string qty { get; set; }
        public string unit { get; set; }
        public string height { get; set; }
        public string width { get; set; }
        public string length { get; set; }
        public string volume { get; set; }
        public string weight { get; set; }
        public string qtyInPallet { get; set; }
        public string rotationRate { get; set; }
        public string abcRank { get; set; }
        public string maxShelfLife { get; set; }
        public string qtySale { get; set; }
        public string unitSale { get; set; }
        public string cost { get; set; }
        public string price { get; set; }
    }

}
