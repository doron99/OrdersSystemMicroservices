using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class OrderWithItemsReponse
    {

        public Guid OrderId { get; set; }

        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; }


        public DateTime CreatedAt { get; set; }

 
        public DateTime UpdatedAt { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
