namespace KoiMuseum.Data.Dtos.Responses.Registration
{
    public class RegistrationResponse
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public decimal? Size { get; set; }

        public int? Age { get; set; }

        public string? ColorPattern { get; set; }

        public string? OwnerName { get; set; }

        public string? Rank { get; set; }

        public string? ContestName { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public DateOnly? ApprovalDate { get; set; }

        public string? RejectedReason { get; set; }

        public string? ConfirmationCode { get; set; }

        public string? IntroductionOfOwner { get; set; }

        public string? IntroductionOfKoi { get; set; }

        public string? Status { get; set; }

        public string? AdminReviewedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UpdatedBy { get; set; }

    }
}
