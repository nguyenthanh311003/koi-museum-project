using KoiMuseum.Data.Base;// Replace with your actual entity namespace
using KoiMuseum.Data.Filters;
using KoiMuseum.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KoiMuseum.Data.Repositories
{
    public class RegistrationRepository : GenericRepository<Registration> // Consider using an interface
    {
        public RegistrationRepository() { }

        public RegistrationRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;

        public async Task<IEnumerable<Registration>> SearchRegistrationsAsync(SearchRegistrationFilter searchRegistrationFilter)
        {
            var query = _context.Registrations.Include(r => r.RegisterDetail)
                                                .ThenInclude(rd => rd.Owner)
                                                .Include(r => r.RegisterDetail)
                                                .ThenInclude(rd => rd.Rank)
                                                .Include(r => r.Contest)
                                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchRegistrationFilter.ownerName))
            {
                query = query.Where(r => r.RegisterDetail.Owner.Name.Contains(searchRegistrationFilter.ownerName));
            }

            if (!string.IsNullOrEmpty(searchRegistrationFilter.contestName))
            {
                query = query.Where(r => r.Contest.Name.Contains(searchRegistrationFilter.contestName));
            }

            if (!string.IsNullOrEmpty(searchRegistrationFilter.status))
            {
                query = query.Where(r => r.Status.Contains(searchRegistrationFilter.status));
            }

            if (!string.IsNullOrEmpty(searchRegistrationFilter.rankName))
            {
                query = query.Where(r => r.RegisterDetail.Rank.Name.Contains(searchRegistrationFilter.rankName));
            }

            if (searchRegistrationFilter.approvalDate.HasValue)
            {
                query = query.Where(r => r.ApprovalDate == searchRegistrationFilter.approvalDate);
            }

            if (!string.IsNullOrEmpty(searchRegistrationFilter.confirmationCode))
            {
                query = query.Where(r => r.ConfirmationCode.Contains(searchRegistrationFilter.confirmationCode));
            }

            return await query.ToListAsync();
        }
    }
}