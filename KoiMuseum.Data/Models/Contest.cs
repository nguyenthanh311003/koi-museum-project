namespace KoiMuseum.Data.Models;

public partial class Contest
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int? NumberOfParticipants { get; set; }

    public int? MaxParticipants { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<ContestProcess> ContestProcesses { get; set; } = new List<ContestProcess>();

    public virtual ICollection<ContestRank> ContestRanks { get; set; } = new List<ContestRank>();

    public virtual ICollection<JudgingResult> JudgingResults { get; set; } = new List<JudgingResult>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();
}
