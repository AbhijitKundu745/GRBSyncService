using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSL.GRB.WMS.Service
{
    public class SalesOrders //OR SalesOrderType OR SalesOrderStatusTypes
    {
        public string totalPages { get; set; }
        public string totalElements { get; set; }
        public Pageable pageable { get; set; }
        public bool first { get; set; }
        public bool last { get; set; }
        public string size { get; set; }
        public List<SalesOrdersContent> content { get; set; }
        public string number { get; set; }
        public Sort sort { get; set; }
        public string numberOfElements { get; set; }
        public bool empty { get; set; }
    }

    public class SalesOrdersContent
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }

        //SalesOrders
        public string salesOrderNo { get; set; }
        public string customerId { get; set; }
        public DateTime orderDate { get; set; }
        public string statusId { get; set; }

        //SalesOrderType
        public string skuCode { get; set; }
        public string skuName { get; set; }
        public string orderQty { get; set; }
        public string qty { get; set; }
        public string netWeight { get; set; }
        public string grossWeight { get; set; }
        public string volume { get; set; }
        public string itemDetails { get; set; }
    }

    public class SalesOrder
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public string salesOrderNo { get; set; }
        public WHContent warehouse { get; set; }
        public Customer customer { get; set; }
        public string orderDate { get; set; }
        public SupplierType status { get; set; }
        public List<ItemsD> items { get; set; }
    }
}
