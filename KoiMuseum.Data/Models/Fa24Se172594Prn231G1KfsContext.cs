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

    public virtual DbSet<ContestRank> ContestRanks { get; set; }

    public virtual DbSet<Judge> Judges { get; set; }

    public virtual DbSet<JudgingResult> JudgingResults { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

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
            entity.HasKey(e => e.Id).HasName("PK__Contest__3213E83F1F025047");

            entity.ToTable("Contest");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.MaxParticipants).HasColumnName("maxParticipants");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.NumberOfParticipants).HasColumnName("numberOfParticipants");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
        });

        modelBuilder.Entity<ContestProcess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ContestP__3213E83F45C36E47");

            entity.ToTable("ContestProcess");

            entity.Property(e => e.Id).HasColumnName("id");
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
            entity.Property(e => e.KoiId).HasColumnName("koiId");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");

            entity.HasOne(d => d.Contest).WithMany(p => p.ContestProcesses)
                .HasForeignKey(d => d.ContestId)
                .HasConstraintName("FK_ContestProcess_Contest");

            entity.HasOne(d => d.Koi).WithMany(p => p.ContestProcesses)
                .HasForeignKey(d => d.KoiId)
                .HasConstraintName("FK_ContestProcess_RegisterDetail");
        });

        modelBuilder.Entity<ContestRank>(entity =>
        {
            entity.HasKey(e => new { e.ContestId, e.RankId });

            entity.ToTable("ContestRank");

            entity.Property(e => e.ContestId).HasColumnName("contestId");
            entity.Property(e => e.RankId).HasColumnName("rankId");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");

            entity.HasOne(d => d.Contest).WithMany(p => p.ContestRanks)
                .HasForeignKey(d => d.ContestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContestRank_Contest");

            entity.HasOne(d => d.Rank).WithMany(p => p.ContestRanks)
                .HasForeignKey(d => d.RankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContestRank_Rank");
        });

        modelBuilder.Entity<Judge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Judge__3213E83F70047D7B");

            entity.ToTable("Judge");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AssignedContests).HasColumnName("assignedContests");
            entity.Property(e => e.Certifications).HasColumnName("certifications");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Experience).HasColumnName("experience");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Judges)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Judge_User");
        });

        modelBuilder.Entity<JudgingResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JudgingR__3213E83F9D801881");

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
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");

            entity.HasOne(d => d.Contest).WithMany(p => p.JudgingResults)
                .HasForeignKey(d => d.ContestId)
                .HasConstraintName("FK_JudgingResult_Contest");

            entity.HasOne(d => d.Judge).WithMany(p => p.JudgingResults)
                .HasForeignKey(d => d.JudgeId)
                .HasConstraintName("FK_JudgingResult_Judge");

            entity.HasOne(d => d.Koi).WithMany(p => p.JudgingResults)
                .HasForeignKey(d => d.KoiId)
                .HasConstraintName("FK_JudgingResult_RegisterDetail");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3213E83F6DFE62A6");

            entity.ToTable("Payment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("currency");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("paymentMethod");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("paymentStatus");
            entity.Property(e => e.RefundAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("refundAmount");
            entity.Property(e => e.RefundStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("refundStatus");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("transactionId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__userId__634EBE90");
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rank__3213E83F8AD3E33C");

            entity.ToTable("Rank");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Criteria).HasColumnName("criteria");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.MaxAge).HasColumnName("maxAge");
            entity.Property(e => e.MaxSize).HasColumnName("maxSize");
            entity.Property(e => e.MinAge).HasColumnName("minAge");
            entity.Property(e => e.MinSize).HasColumnName("minSize");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Reward)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("reward");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
            entity.Property(e => e.VarietyRestriction)
                .HasMaxLength(255)
                .HasColumnName("varietyRestriction");
        });

        modelBuilder.Entity<RegisterDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Register__3213E83F7E8C3807");

            entity.ToTable("RegisterDetail");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdDate");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .HasColumnName("gender");
            entity.Property(e => e.ImageUrl).HasColumnName("imageUrl");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.OwnerId).HasColumnName("ownerId");
            entity.Property(e => e.RankId).HasColumnName("rankId");
            entity.Property(e => e.Size)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("size");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
            entity.Property(e => e.Weight).HasColumnName("weight");

            entity.HasOne(d => d.Owner).WithMany(p => p.RegisterDetails)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_RegisterDetail_User");

            entity.HasOne(d => d.Rank).WithMany(p => p.RegisterDetails)
                .HasForeignKey(d => d.RankId)
                .HasConstraintName("FK_RegisterDetail_Rank");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Registra__3213E83F7057616A");

            entity.ToTable("Registration");

            entity.HasIndex(e => e.RegisterDetailId, "UQ__Registra__E697F4C420E75971").IsUnique();

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
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("registrationDate");
            entity.Property(e => e.RejectedReason).HasColumnName("rejectedReason");
            entity.Property(e => e.Remark)
                .HasMaxLength(10)
                .HasColumnName("remark");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");

            entity.HasOne(d => d.Contest).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.ContestId)
                .HasConstraintName("FK_Registration_Contest");

            entity.HasOne(d => d.RegisterDetail).WithOne(p => p.Registration)
                .HasForeignKey<Registration>(d => d.RegisterDetailId)
                .HasConstraintName("FK_Registration_RegisterDetail");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83FD2DCB6FF");

            entity.ToTable("User");

            entity.HasIndex(e => e.Name, "UQ__User__72E12F1BEA74CEAA").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__AB6E616471EBBE1E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatarUrl");
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
                .HasMaxLength(20)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updatedDate");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
