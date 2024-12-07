using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class Warehouses
    {
        public List<WHContent> content { get; set; }
        public Pageable pageable { get; set; }
        public string totalPages { get; set; }
        public string totalElements { get; set; }
        public string unpaged { get; set; }
        public string paged { get; set; }
        public string pageSize { get; set; }
        public string pageNumber { get; set; }
        public string offset { get; set; }
        public Sort sort { get; set; }
        public bool first { get; set; }
        public bool last { get; set; }
        public bool size { get; set; }
        public string numberOfElements { get; set; }
        public bool empty { get; set; }
        public bool sorted { get; set; }
        public bool unsorted { get; set; }
    }

    public class WHContent
    {
        //public string createdBy { get; set; }
        //public DateTime createdAt { get; set; }
        //public string updatedBy { get; set; }
        //public DateTime updatedAt { get; set; }
        public int id { get; set; }
        public string plantNumber { get; set; }
        public string name { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string pin { get; set; }
        public string countryCode { get; set; }
        public string regionCode { get; set; }
        //public string warehouseManagerUserId { get; set; }
        //public string contactEmail { get; set; }
        //public string contactPhone { get; set; }

    }
}
