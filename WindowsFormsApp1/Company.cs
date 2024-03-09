using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class Company
    {
        public string totalPages { get; set; }
        public string totalElements { get; set; }
        public Pageable pageable { get; set; }
        public bool first { get; set; }
        public bool last { get; set; }
        public string size { get; set; }
        public List<CompanyContent> content { get; set; }
        public string number { get; set; }
        public Sort sort { get; set; }
        public string numberOfElements { get; set; }
        public bool empty { get; set; }
    }

    public class CompanyContent
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public string companyName { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string managerUserId { get; set; }
        public string phone { get; set; }
    }
}
