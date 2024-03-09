using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class Bays
    {
        public List<BaysContent> content { get; set; }
        public Pageable pageable { get; set; }
        public string totalPages { get; set; }
        public string totalElements { get; set; }
        public bool first { get; set; }
        public bool last { get; set; }
        public bool size { get; set; }
        public int number { get; set; }
        public Sort sort { get; set; }
        public string numberOfElements { get; set; }
        public bool empty { get; set; }

        //public string unpaged { get; set; }
        //public string paged { get; set; }
        //public string pageSize { get; set; }
        //public string pageNumber { get; set; }
        //public string offset { get; set; }
        //public bool sorted { get; set; }
        //public bool unsorted { get; set; }
    }

    public class BaysContent
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public string bayName { get; set; }
        public string parentId { get; set; }
        public Zone zone { get; set; }
        public BayTypesContent bayType { get; set; }
        public WHContent warehouse { get; set; }
        public aisle aisle { get; set; }
    }
}
