using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace KoiMuseum.Data.Models;

public partial class Fa24Se172594Prn231G1KfsContext : DbContext
{
    public Fa24Se172594Prn231G1KfsContext()
    {
    }

    public Fa24Se172594Prn231G1KfsContext(DbContextOptions<Fa24Se172594Prn231G1KfsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contest> Contests { get; set; }

    public virtual DbSet<ContestProcess> ContestProcesses { get; set; }

    public virtual DbSet<Judge> Judges { get; set; }

    public virtual DbSet<JudgingResult> JudgingResults { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    public virtual DbSet<RegisterDetail> RegisterDetails { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=(local);database=FA24_SE172594_PRN231_G1_KFS;uid=sa;pwd=12345;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contest__3213E83F6BF86014");

            entity.ToTable("Contest");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Criteria)
                .HasMaxLength(255)
                .HasColumnName("criteria");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.MaxParticipants).HasColumnName("maxParticipants");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.NumberOfParticipants).HasColumnName("numberOfParticipants");
            entity.Property(e => e.Organizer)
                .HasMaxLength(255)
                .HasColumnName("organizer");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
        });

        modelBuilder.Entity<ContestProcess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContestP__3213E83FA4320983");

            entity.ToTable("ContestProcess");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssignedBy)
                .HasMaxLength(255)
                .HasColumnName("assignedBy");
            entity.Property(e => e.AssignedRank)
                .HasMaxLength(255)
                .HasColumnName("assignedRank");
            entity.Property(e => e.CheckInStatus)
                .HasMaxLength(255)
                .HasColumnName("checkInStatus");
            entity.Property(e => e.CompetitionStage)
                .HasMaxLength(255)
                .HasColumnName("competitionStage");
            entity.Property(e => e.ContestId).HasColumnName("contestId");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.FinalResult)
                .HasMaxLength(255)
                .HasColumnName("finalResult");
            entity.Property(e => e.KoiId).HasColumnName("koiId");
            entity.Property(e => e.Stage)
                .HasMaxLength(255)
                .HasColumnName("stage");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");

            entity.HasOne(d => d.Contest).WithMany(p => p.ContestProcesses)
                .HasForeignKey(d => d.ContestId)
                .HasConstraintName("FK__ContestPr__conte__4BAC3F29");

            entity.HasOne(d => d.Koi).WithMany(p => p.ContestProcesses)
                .HasForeignKey(d => d.KoiId)
                .HasConstraintName("FK__ContestPr__koiId__4CA06362");
        });

        modelBuilder.Entity<Judge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Judge__3213E83FC5E4C56B");

            entity.ToTable("Judge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssignedContests).HasColumnName("assignedContests");
            entity.Property(e => e.Availability)
                .HasMaxLength(255)
                .HasColumnName("availability");
            entity.Property(e => e.Certifications).HasColumnName("certifications");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Experience).HasColumnName("experience");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Specialty)
                .HasMaxLength(255)
                .HasColumnName("specialty");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Judges)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Judge__userId__52593CB8");
        });

        modelBuilder.Entity<JudgingResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JudgingR__3213E83F64327D58");

            entity.ToTable("JudgingResult");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ColorScore)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("colorScore");
            entity.Property(e => e.ContestId).HasColumnName("contestId");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.JudgeId).HasColumnName("judgeId");
            entity.Property(e => e.KoiId).HasColumnName("koiId");
            entity.Property(e => e.PatternScore)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("patternScore");
            entity.Property(e => e.Remarks).HasColumnName("remarks");
            entity.Property(e => e.ReviewedAt)
                .HasColumnType("datetime")
                .HasColumnName("reviewedAt");
            entity.Property(e => e.Score)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("score");
            entity.Property(e => e.ShapeScore)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("shapeScore");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");

            entity.HasOne(d => d.Contest).WithMany(p => p.JudgingResults)
                .HasForeignKey(d => d.ContestId)
                .HasConstraintName("FK__JudgingRe__conte__5EBF139D");

            entity.HasOne(d => d.Judge).WithMany(p => p.JudgingResults)
                .HasForeignKey(d => d.JudgeId)
                .HasConstraintName("FK__JudgingRe__judge__5CD6CB2B");

            entity.HasOne(d => d.Koi).WithMany(p => p.JudgingResults)
                .HasForeignKey(d => d.KoiId)
                .HasConstraintName("FK__JudgingRe__koiId__5DCAEF64");
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rank__3213E83FE309956F");

            entity.ToTable("Rank");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Criteria)
                .HasMaxLength(255)
                .HasColumnName("criteria");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.MaxAge).HasColumnName("maxAge");
            entity.Property(e => e.MaxSize).HasColumnName("maxSize");
            entity.Property(e => e.MinAge).HasColumnName("minAge");
            entity.Property(e => e.MinSize).HasColumnName("minSize");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Reward)
                .HasMaxLength(255)
                .HasColumnName("reward");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
            entity.Property(e => e.VarietyRestriction)
                .HasMaxLength(255)
                .HasColumnName("varietyRestriction");
        });

        modelBuilder.Entity<RegisterDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Register__3213E83FF30735F1");

            entity.ToTable("RegisterDetail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.ColorPattern).HasColumnName("colorPattern");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.HealthStatus)
                .HasMaxLength(255)
                .HasColumnName("healthStatus");
            entity.Property(e => e.OwnerId).HasColumnName("ownerId");
            entity.Property(e => e.Picture)
                .HasMaxLength(255)
                .HasColumnName("picture");
            entity.Property(e => e.RankId).HasColumnName("rankId");
            entity.Property(e => e.Size)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("size");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
            entity.Property(e => e.Variety)
                .HasMaxLength(255)
                .HasColumnName("variety");

            entity.HasOne(d => d.Owner).WithMany(p => p.RegisterDetails)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__RegisterD__owner__4316F928");

            entity.HasOne(d => d.Rank).WithMany(p => p.RegisterDetails)
                .HasForeignKey(d => d.RankId)
                .HasConstraintName("FK__RegisterD__rankI__4222D4EF");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Registra__3213E83FEFB837F4");

            entity.ToTable("Registration");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdminReviewedBy)
                .HasMaxLength(255)
                .HasColumnName("adminReviewedBy");
            entity.Property(e => e.ApprovalDate).HasColumnName("approvalDate");
            entity.Property(e => e.ConfirmationCode)
                .HasMaxLength(255)
                .HasColumnName("confirmationCode");
            entity.Property(e => e.ContestId).HasColumnName("contestId");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.IntroductionOfKoi).HasColumnName("introductionOfKoi");
            entity.Property(e => e.IntroductionOfOwner).HasColumnName("introductionOfOwner");
            entity.Property(e => e.RegisterDetailId).HasColumnName("registerDetailId");
            entity.Property(e => e.RegistrationDate).HasColumnName("registrationDate");
            entity.Property(e => e.RejectedReason).HasColumnName("rejectedReason");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");

            entity.HasOne(d => d.Contest).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.ContestId)
                .HasConstraintName("FK__Registrat__conte__571DF1D5");

            entity.HasOne(d => d.RegisterDetail).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.RegisterDetailId)
                .HasConstraintName("FK__Registrat__koiId__5812160E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83FCB361A0B");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E6164461AB113").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .HasColumnName("avatarUrl");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(NULL)")
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
