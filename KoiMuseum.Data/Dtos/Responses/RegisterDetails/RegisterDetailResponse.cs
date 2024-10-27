using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Dtos.Responses.RegisterDetails
{
    public class RegisterDetailResponse
    {
        public int Id { get; set; }
        public string? RankName { get; set; }

        public string? OwnerName { get; set; }

        public decimal? Size { get; set; }

        public int? Age { get; set; }

        public string? Type { get; set; }

        public string? Gender { get; set; }

        public string? Status { get; set; }

        public string? ImageUrl { get; set; }

        public string? Name { get; set; }

        public string? Weight { get; set; }
    }
}
