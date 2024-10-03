using System;
using System.Collections.Generic;

namespace KoiMuseum.Data.Models;

public partial class Registration
{
    public int Id { get; set; }

    public int? ContestId { get; set; }

    public int? RegisterDetailId { get; set; }

    public string? Status { get; set; }

    public DateOnly? RegistrationDate { get; set; }

    public DateOnly? ApprovalDate { get; set; }

    public string? RejectedReason { get; set; }

    public string? ConfirmationCode { get; set; }

    public string? IntroductionOfOwner { get; set; }

    public string? IntroductionOfKoi { get; set; }

    public string? AdminReviewedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Contest? Contest { get; set; }

    public virtual RegisterDetail? RegisterDetail { get; set; }
}
