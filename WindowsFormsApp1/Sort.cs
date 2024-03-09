using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class Sort
    {
        public bool sorted { get; set; }
        public bool unsorted { get; set; }
        public bool empty { get; set; }
       
    }

    public class DispatchSort
    {
        public string direction { get; set; }
        public string property { get; set; }
        public bool ignoreCase { get; set; }
        public string nullHandling { get; set; }
        public bool ascending { get; set; }
        public bool descending { get; set; }
    }
}
