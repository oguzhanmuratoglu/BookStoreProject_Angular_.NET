namespace BookStoreServer.WebApi.Enums;

public enum OrderStatusEnum
{
    AwaitingApproval,
    Preparing,
    InTransit,
    Delivered,
    Rejected,
    Returned
}
