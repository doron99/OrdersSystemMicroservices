using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class OrderAddRequest
    {
        [Required(ErrorMessage = "CustomerId is required.")]
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
