using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class ReceivingByTrucks
    {
        public string totalPages { get; set; }
        public string totalElements { get; set; }
        public Pageable pageable { get; set; }
        public bool first { get; set; }
        public bool last { get; set; }
        public string size { get; set; }
        public List<ReceivingByTrucksContent> content { get; set; }
        public string number { get; set; }
        public List<DispatchSort> sort { get; set; }
        public string numberOfElements { get; set; }
        public bool empty { get; set; }
    }

    public class ReceivingByTrucksContent
    {
        public string truckNo { get; set; }
        public List<ReceivingContent> receivings { get; set; }
    }
}
