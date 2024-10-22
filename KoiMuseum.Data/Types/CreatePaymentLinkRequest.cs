namespace NetCoreDemo.Types;

public record CreatePaymentLinkRequest(
    int userId,
    string productName,
    string description,
    int price
);