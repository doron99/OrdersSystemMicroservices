using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class OrderReponse
    {

        public Guid Id { get; set; }

        public string CustomerId { get; set; }

        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; }


        public DateTime CreatedAt { get; set; }

 
        public DateTime UpdatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
