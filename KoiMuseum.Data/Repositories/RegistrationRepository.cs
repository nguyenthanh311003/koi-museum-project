using KoiMuseum.Data.Base; // Replace with your actual base repository namespace
using KoiMuseum.Data.Models;

namespace KoiMuseum.Data.Repositories
{
    public class RegistrationRepository : GenericRepository<Registration>
    {
        public RegistrationRepository() { }

        public RegistrationRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;
    }
}
