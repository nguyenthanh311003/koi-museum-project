using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Dtos.Requests.Ranks
{
    public class UpdateRankRequest
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? MinSize { get; set; }

        public string? Criteria { get; set; }

        public decimal? Reward { get; set; }

        public int? MaxSize { get; set; }

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public string? VarietyRestriction { get; set; }
    }
}
