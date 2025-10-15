using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client.API.Dtos
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public string ProductDesc { get; set; }
        public string Sku { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
    }
}
