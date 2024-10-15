namespace Net.payOS.Types;

public record CreatePaymentResult(
    String bin,
    String accountNumber,
    int amount,
    String description,
    long orderCode,
    String paymentLinkId,
    String status,
    String checkoutUrl,
    String qrCode
);