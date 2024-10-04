using System;
using System.Collections.Generic;

namespace KoiMuseum.Data.Models;

public partial class ContestRank
{
    public int ContestId { get; set; }

    public int RankId { get; set; }

    public string? Status { get; set; }

    public virtual Contest Contest { get; set; } = null!;

    public virtual Rank Rank { get; set; } = null!;
}
