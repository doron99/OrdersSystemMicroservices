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
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        [Required]
        public string ProductDesc { get; set; }
        [Required]
        public string Sku { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        [Required]
        public int Stock { get; set; }
        public static Product Create(string productDesc, string sku, decimal unitPrice, int stock)
        {
            if (stock < 1)
            {
                throw new ArgumentException("Quantity must be at least 1.", nameof(stock));
            }

            return new Product
            {
                ProductId = Guid.NewGuid(),
                ProductDesc = productDesc,
                Sku = sku,
                UnitPrice = unitPrice,
                Stock = stock
            };
        }
    }

        
}
