namespace Client.API.Models
{
    //this is duplicate because it perform a different microservice that consume same data structure
    public class OrderForResponseWithItems
    {
        public Guid OrderId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }
    }
    public class OrderItemDto
    {
        public Guid OrderItemId { get; set; }
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
    public enum OrderStatus
    {
        Draft = 0,
        Confirmed = 1,
        Cancelled = 2
    }
}
