using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Dtos.Requests.ContestRank
{
    public class CreateContestRank
    {
        public int ContestId { get; set; }

        public int RankId { get; set; }

        public string? Status { get; set; }
    }
}
