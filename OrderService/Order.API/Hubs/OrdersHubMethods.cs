namespace Order.API.Hubs
{
    public static class OrdersHubMethods
    {
        public const string ReceiveOrderStatusUpdate = "ReceiveOrderStatusUpdate";
        public const string ReceiveOrderCreate = "ReceiveOrderCreate";
        public const string ReceiveOrderItemDeleted = "ReceiveOrderItemDeleted";
        public const string ReceiveOrderItemCreate = "ReceiveOrderItemCreate";
        public const string ReceiveOrderConfirmed = "ReceiveOrderConfirmed";

    }
}
