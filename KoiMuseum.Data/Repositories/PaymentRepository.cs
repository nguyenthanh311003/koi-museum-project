using KoiMuseum.Data.Base;
using KoiMuseum.Data.Models;

namespace KoiMuseum.Data.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository() { }
        public PaymentRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;
    }
}
