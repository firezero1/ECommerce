using System;
using System.Collections.Generic;

namespace ECommerce.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public string Customer { get; set; }
        public string Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public string Remark { get; set; }
        public string StatusDescription { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; } = new List<OrderDetailViewModel>();
    }

    public class OrderDetailViewModel
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
