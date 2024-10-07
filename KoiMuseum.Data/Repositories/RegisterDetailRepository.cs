using KoiMuseum.Data.Base;
using KoiMuseum.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiMuseum.Data.Repositories
{
    public class RegisterDetailRepository : GenericRepository<RegisterDetail> // Consider using an interface
    {
        public RegisterDetailRepository() { }

        public RegisterDetailRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;

        public async Task<int> CountRegisterDetailByRankId(int id)
        {
            return await _context.RegisterDetails.CountAsync(x => x.RankId == id);
        }

        public async Task<List<RegisterDetail>> getRegisterDetailsByRankId(int rankId)
        {
            return await _context.RegisterDetails
                .Where(x => x.RankId == rankId)
                .ToListAsync();
        }
    }
}
