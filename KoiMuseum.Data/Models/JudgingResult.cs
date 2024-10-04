using System;
using System.Collections.Generic;

namespace KoiMuseum.Data.Models;

public partial class JudgingResult
{
    public int Id { get; set; }

    public int? JudgeId { get; set; }

    public int? KoiId { get; set; }

    public int? ContestId { get; set; }

    public decimal? Score { get; set; }

    public decimal? ShapeScore { get; set; }

    public decimal? ColorScore { get; set; }

    public decimal? PatternScore { get; set; }

    public string? Remarks { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual Contest? Contest { get; set; }

    public virtual Judge? Judge { get; set; }

    public virtual RegisterDetail? Koi { get; set; }
}
