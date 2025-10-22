namespace Client.API.Dtos
{
    public class AddOrderRequest
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
