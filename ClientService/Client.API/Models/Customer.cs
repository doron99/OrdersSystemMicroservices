namespace Client.API.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; } = Guid.NewGuid();

        public string CustomerName { get; set; }
    }
    
}
