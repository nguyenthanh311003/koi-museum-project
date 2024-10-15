using System;
using System.Collections.Generic;

namespace KoiMuseum.Data.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public string TransactionId { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public string? Description { get; set; }

    public string? RefundStatus { get; set; }

    public decimal? RefundAmount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
