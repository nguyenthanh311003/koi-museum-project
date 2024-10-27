using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Dtos.Responses.Judge
{
    public class JudgeDto
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? Experience { get; set; }

        public string? Certifications { get; set; }

        public string? AssignedContests { get; set; }

        public string? Status { get; set; }
    }
}
