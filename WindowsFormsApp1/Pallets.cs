using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class Pallets
    {
        public List<PalletsContent> content { get; set; }
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

    public class PalletsContent
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public string warehouseId { get; set; }
        public string name { get; set; }
        public string length { get; set; }
        public string width { get; set; }
        public string height { get; set; }
        public string maxWeight { get; set; }
        public string sensorId { get; set; }
    }
}
