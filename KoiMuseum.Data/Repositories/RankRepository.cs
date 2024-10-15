using KoiMuseum.Data.Base;
using KoiMuseum.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Repositories
{
    public class RankRepository : GenericRepository<Rank>
    {
        public RankRepository() { }

        public RankRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;

        public async Task<Rank> GetRankByNameAsync(string rankName)
        {
            return _context.Ranks.FirstOrDefault(r => r.Name.Equals(rankName));
        }
    }
}
