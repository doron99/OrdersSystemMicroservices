using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class StockTracking
    {
        [Key]
        public Guid StockTrackingId { get; set; }
        public string? SKU { get; set; }
        public int? StockBeforeAction { get; set; }
        public int? WithdrawalQuantity { get; set; }
        public int? RestockQuantity { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Remarks { get; set; }
        public string? Type { get; set; }
        public Guid? OrderId { get; set; }
        public StockTracking()
        {
            StockTrackingId = Guid.NewGuid();
        }
        public StockTracking(string sku, int stockBeforeAction,int withdrawalQuantity, int restockQuantity, DateTime createDate, string remarks, Guid? orderId)
        {
            SKU = sku;
            StockBeforeAction = stockBeforeAction;
            WithdrawalQuantity = withdrawalQuantity;
            RestockQuantity = restockQuantity;
            CreateDate = createDate;
            Remarks = remarks;
            OrderId = orderId ?? null;

        }

    }
}
