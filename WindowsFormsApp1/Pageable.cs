using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class Pageable
    {
        public bool unpaged { get; set; }
        public string pageNumber { get; set; }
        public string pageSize { get; set; }
        public bool paged { get; set; }
        public string offset { get; set; }
        public List<DispatchSort> sort { get; set; } 

    }

    public class DispatchPageable
    {
        public bool unpaged { get; set; }
        public string pageNumber { get; set; }
        public string pageSize { get; set; }
        public bool paged { get; set; }
        public string offset { get; set; }
        public List<DispatchSort> sort { get; set; }

    }
}
