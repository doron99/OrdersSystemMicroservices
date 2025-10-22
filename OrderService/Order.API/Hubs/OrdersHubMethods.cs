namespace Order.API.Hubs
{
    public static class OrdersHubMethods
    {
        public const string ReceiveOrderStatusUpdate = "ReceiveOrderStatusUpdate";
        public const string ReceiveOrderCreate = "ReceiveOrderCreate";
        public const string ReceiveOrderItemDeleted = "ReceiveOrderItemDeleted";
        public const string ReceiveOrderItemCreate = "ReceiveOrderItemCreate";
        public const string ReceiveOrderConfirmed = "ReceiveOrderConfirmed";

        //messages
        public const string MsgConfirmAndWaitingApproval = "The request has been confirmed and is awaiting approval";
        public const string MsgCanceled = "The order has been canceled";



    }
}
