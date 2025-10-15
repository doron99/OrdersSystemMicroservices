using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public enum OrderStatus
    {
        Draft = 0,
        Confirmed = 1,
        Cancelled = 2
    }
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        [NotMapped]
        public decimal TotalAmount => CalculateTotalAmount();

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public Order()
        {
            Id = Guid.NewGuid(); // Generate a new GUID
            CreatedAt = DateTime.UtcNow; // Set creation date
            UpdatedAt = DateTime.UtcNow; // Set updated date
            OrderItems = new List<OrderItem>();
        }
        

        private decimal CalculateTotalAmount()
        {
            decimal total = 0;
            foreach (var item in OrderItems)
            {
                total += item.Quantity * item.UnitPrice;
            }
            return total;
        }
    }
}
