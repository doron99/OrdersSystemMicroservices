using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{

    public class OrderItem
    {
        public OrderItem()
        {
            OrderItemId = Guid.NewGuid();
        }
        [Key]
        public Guid OrderItemId { get; set; }

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public string OrderItemDesc { get; set; }
        [Required]
        public string Sku { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        [JsonIgnore]
        public virtual Order Order { get; set; }
    }
}
