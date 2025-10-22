namespace Order.API.DTOs
{
    public class OrderToApproveMessageResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public Guid OrderId { get; set; }
    }
}
