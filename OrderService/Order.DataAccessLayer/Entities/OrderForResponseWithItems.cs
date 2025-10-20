using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class OrderForResponseWithItems
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public Guid CustomerId { get; set; }    
        public OrderStatus Status { get; set; }           
        public DateTime CreatedAt { get; set; }   
        public DateTime UpdatedAt { get; set; }   
        public decimal TotalAmount { get; set; }  
        public ICollection<OrderItemDto> OrderItems { get; set; } 
    }
    public class OrderItemDto
    {
        public Guid OrderItemId { get; set; }
        public Guid ProductId { get; set; }
        public string OrderItemDesc { get; set; }
        public string Sku { get; set; }       
        public int Quantity { get; set; }     
        public decimal UnitPrice { get; set; }
    }

}
