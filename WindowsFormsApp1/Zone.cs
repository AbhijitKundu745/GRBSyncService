using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class Zone
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public WHContent warehouse { get; set; }
    }

    public class aisle
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public WHContent warehouse { get; set; }
        public string length { get; set; }
        public string lengthUnit { get; set; }
        public string width { get; set; }
        public string widthUnit { get; set; }
        public string height { get; set; }
        public string heightUnit { get; set; }
        public string startLatitude { get; set; }
        public string startLongitude { get; set; }
        public string endLatitude { get; set; }
        public string endLongitude { get; set; }

    }
}
