namespace KoiMuseum.Data.Dtos.Requests.PayOS
{
    public record PaymentData(
        long OrderCode,
        int Amount,
        string Description,
        List<ItemData> Items,
        string CancelUrl,
        string ReturnUrl,
        string? Signature = null,
        string? BuyerName = null,
        string? BuyerEmail = null,
        string? BuyerPhone = null,
        string? BuyerAddress = null,
        int? ExpiredAt = null
    );

    public record ItemData(
        string Name,
        int Quantity,
        int Price
    );
}
