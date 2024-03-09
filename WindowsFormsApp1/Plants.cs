using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class Plants
    {
        public string totalPages { get; set; }
        public string totalElements { get; set; }
        public Pageable pageable { get; set; }
        public bool first { get; set; }
        public bool last { get; set; }
        public string size { get; set; }
        public List<PlantsContent> content { get; set; }
        public string number { get; set; }
        public Sort sort { get; set; }
        public string numberOfElements { get; set; }
        public bool empty { get; set; }
    }

    public class PlantsContent
    {
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string record_status { get; set; }
        public string id { get; set; }
        public string city { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string plant_code { get; set; }
        public string street { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
        public string region { get; set; }
    }
}
