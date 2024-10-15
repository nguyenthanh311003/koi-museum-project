using KoiMuseum.Data.Base;
using KoiMuseum.Data.Models;

namespace KoiMuseum.Data.Repositories
{
    public class JudgeResultRepository : GenericRepository<Rank>
    {
        public JudgeResultRepository() { }

        public JudgeResultRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;
    }
}
