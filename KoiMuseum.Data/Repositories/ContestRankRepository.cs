using KoiMuseum.Data.Base;
using KoiMuseum.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Repositories
{
    public class ContestRankRepository : GenericRepository<ContestRank>
    {
        public ContestRankRepository() { }

        public ContestRankRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;

        public async Task<ContestRank> GetContestRankByContestIdAndRankId(int contestId, int rankId)
        {
            return await _context.ContestRanks.FirstOrDefaultAsync(cr => cr.ContestId == contestId && cr.RankId == rankId);
        }
    }
}
