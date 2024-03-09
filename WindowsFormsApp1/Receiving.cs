using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class Receiving
    {
       
        public string totalPages { get; set; }
        public string totalElements { get; set; }
        public Pageable pageable { get; set; }
        public bool first { get; set; }
        public bool last { get; set; }
        public string size { get; set; }
        public List<ReceivingContent> content { get; set; }
        public string number { get; set; }
        public Sort sort { get; set; }
        public string numberOfElements { get; set; }
        public bool empty { get; set; }
    }

    public class ReceivingContent
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public string receivingNo { get; set; }
        public Supplier supplier { get; set; }
        public WHContent warehouse { get; set; }
        public SupplierType status { get; set; }
        public string truckNumber { get; set; }
        public string eta { get; set; }
        public List<Items> items { get; set; }

    }

    //public class PUTSuggestions
    //{
    //    public string createdBy { get; set; }
    //    public DateTime createdAt { get; set; }
    //    public string updatedBy { get; set; }
    //    public DateTime updatedAt { get; set; }
    //    public string id { get; set; }

    //}

    //public class PalletR
    //{
    //    public string createdBy { get; set; }
    //    public DateTime createdAt { get; set; }
    //    public string updatedBy { get; set; }
    //    public DateTime updatedAt { get; set; }
    //    public string id { get; set; }
    //    public PalletType palletType { get; set; }

    //}

    //public class PalletType
    //{

    //}


 }
