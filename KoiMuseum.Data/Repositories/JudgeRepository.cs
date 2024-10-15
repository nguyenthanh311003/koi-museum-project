using KoiMuseum.Data.Base;
using KoiMuseum.Data.Models;

namespace KoiMuseum.Data.Repositories
{
    public class JudgeRepository : GenericRepository<Rank>
    {
        public JudgeRepository() { }

        public JudgeRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;
    }
}
