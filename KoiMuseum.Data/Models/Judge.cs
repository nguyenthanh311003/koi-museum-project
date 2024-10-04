using System;
using System.Collections.Generic;

namespace KoiMuseum.Data.Models;

public partial class Judge
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? Experience { get; set; }

    public string? Certifications { get; set; }

    public string? AssignedContests { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<JudgingResult> JudgingResults { get; set; } = new List<JudgingResult>();

    public virtual User? User { get; set; }
}
