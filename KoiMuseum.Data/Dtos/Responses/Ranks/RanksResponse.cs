using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Dtos.Responses.Ranks
{
    public class RanksResponse
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string ContestName {  get; set; } = null!;

        public string Criteria { get; set; } = null!;

        public decimal Reward { get; set; }

        public string? Description { get; set; }

        public int? MinSize { get; set; }

        public int? MaxSize { get; set; }

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public int Participants { get; set; }

        public string Status { get; set; }

        public string? VarietyRestriction { get; set; }
    }
}
