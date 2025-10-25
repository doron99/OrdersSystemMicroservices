namespace Order_Common
{
    public class OrderToApproveMessageResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public Guid OrderId { get; set; }
    }
}
