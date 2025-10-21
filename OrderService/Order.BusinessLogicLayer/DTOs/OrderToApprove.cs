using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.BusinessLogicLayer.DTOs
{
    public class OrderToApprove
    {
        public Guid OrderId { get; set; }
        public List<ProductToApprove> Products { get; set; }
    }

    public class ProductToApprove
    {
        public string Sku { get; set; }
        public int Quantity { get; set; }
    }
}
