using System;
using System.Collections.Generic;

namespace KoiMuseum.Data.Models;

public partial class RegisterDetail
{
    public int Id { get; set; }

    public int? RankId { get; set; }

    public int? OwnerId { get; set; }

    public string? Variety { get; set; }

    public string? Picture { get; set; }

    public decimal? Size { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public string? ColorPattern { get; set; }

    public string? HealthStatus { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<ContestProcess> ContestProcesses { get; set; } = new List<ContestProcess>();

    public virtual ICollection<JudgingResult> JudgingResults { get; set; } = new List<JudgingResult>();

    public virtual User? Owner { get; set; }

    public virtual Rank? Rank { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
