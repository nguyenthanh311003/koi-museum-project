using KoiMuseum.Data.Base;
using KoiMuseum.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiMuseum.Data.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository() { }
        public PaymentRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;
        public async Task<Payment?> GetByTransactionIdAsync(string transactionId)
        {
            return await _context.Set<Payment>()
                                 .FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }
    }
}
