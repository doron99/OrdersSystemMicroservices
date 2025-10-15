using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class OrderItemAddRequest
    {
        public string Sku { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
