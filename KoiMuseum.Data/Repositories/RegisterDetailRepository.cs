using KoiMuseum.Data.Base;
using KoiMuseum.Data.Models;

namespace KoiMuseum.Data.Repositories
{
    public class RegisterDetailRepository : GenericRepository<RegisterDetail> // Consider using an interface
    {
        public RegisterDetailRepository() { }

        public RegisterDetailRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;
    }
}
