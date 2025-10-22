using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class OrderToApproveMessageResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public Guid OrderId { get; set; }
    }
}
