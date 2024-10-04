using System;
using System.Collections.Generic;

namespace KoiMuseum.Data.Models;

public partial class Rank
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? MinSize { get; set; }

    public string? Criteria { get; set; }

    public decimal? Reward { get; set; }

    public int? MaxSize { get; set; }

    public int? MinAge { get; set; }

    public int? MaxAge { get; set; }

    public string? VarietyRestriction { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<ContestRank> ContestRanks { get; set; } = new List<ContestRank>();

    public virtual ICollection<RegisterDetail> RegisterDetails { get; set; } = new List<RegisterDetail>();
}
