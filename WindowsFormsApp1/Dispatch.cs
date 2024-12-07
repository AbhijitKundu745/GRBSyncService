using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class Dispatch
    {
        public string totalPages { get; set; }
        public string totalElements { get; set; }
        public DispatchPageable pageable { get; set; }
        public bool first { get; set; }
        public bool last { get; set; }
        public string size { get; set; }
        public List<DispatchData> content { get; set; }
        public string number { get; set; }
        public List<DispatchSort> sort { get; set; }
        public string numberOfElements { get; set; }
        public bool empty { get; set; }
    }

    public class DispatchContent //
    {
        public string truckNo { get; set; }
        public List<DispatchData> dispatches { get; set; }
    }
    public class DispatchData
    {
        public int createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public int updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string recordStatus { get; set; }
        public int id { get; set; }
        public string dispatchNo { get; set; }
        public DateTime expectedDeliveryDate { get; set; }
        public WHContent warehouse { get; set; }
        public SupplierType status { get; set; }
        public string billingDocument { get; set; }
        public string billingType { get; set; }
        public DateTime billingDate { get; set; }
        public string payerCode { get; set; }
        public string payerName { get; set; }
        public string customerGroup { get; set; }
        public string truckNumber { get; set; }
        public double freightCharges { get; set; }
        public double otherCharges { get; set; }
        public string pgi { get; set; }
        public List<DispatchItems> items { get; set; }
        //public List<PickSuggestion> pickSuggestions { get; set; }
    }

    public class DispatchItems
    {
        public int createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public int updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public int id { get; set; }
        //public string orderNo { get; set; }
        //public string orderType { get; set; }
        public string orderItemNo { get; set; }
        //public SalesOrder salesOrder { get; set; } //
        //public ItemsD salesOrderItem { get; set; }//
        public SKUContent sku { get; set; }
        public string skuCode { get; set; }
        public string skuName { get; set; }
        public string recordStatus { get; set; }
        //public string orderQty { get; set; }
        public double qty { get; set; }
        public double loadedQty { get; set; }
        //public string weight { get; set; }
        public double netWeight { get; set; }
        public double grossWeight { get; set; }
        public string volume { get; set; }
        public string volumeUnit { get; set; }
    }
    public class PickSuggestion
    {
        public int createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int id { get; set; }
        public SKUContent sku { get; set; }
        public FromBin fromBin { get; set; }
        public ToBin toBin { get; set; }
        public double qty { get; set; }
        public string solutionMethod { get; set; }
        public string batchNumber { get; set; }
    }


}
