using System;
using System.Collections.Generic;

namespace KoiMuseum.Data.Models;

public partial class ContestProcess
{
    public int Id { get; set; }

    public int? ContestId { get; set; }

    public int? KoiId { get; set; }

    public string? Status { get; set; }

    public string? Stage { get; set; }

    public string? AssignedRank { get; set; }

    public string? CheckInStatus { get; set; }

    public string? AssignedBy { get; set; }

    public string? CompetitionStage { get; set; }

    public string? FinalResult { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Contest? Contest { get; set; }

    public virtual RegisterDetail? Koi { get; set; }
}
