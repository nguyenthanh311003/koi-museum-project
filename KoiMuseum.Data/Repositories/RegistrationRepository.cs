using KoiMuseum.Data.Base;
using KoiMuseum.Data.Filters;
using KoiMuseum.Data.Models;
using KoiMuseum.Data.PagingModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KoiMuseum.Data.Repositories
{
    public class RegistrationRepository : GenericRepository<Registration>
    {
        public RegistrationRepository() { }

        public RegistrationRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;

        public async Task<PagedResult<Registration>> SearchRegistrationsPagedAsync(SearchRegistrationFilter filter, int pageNumber, int pageSize)
        {
            var query = _context.Registrations
                                .Include(r => r.RegisterDetail)
                                    .ThenInclude(rd => rd.Owner)
                                .Include(r => r.RegisterDetail)
                                    .ThenInclude(rd => rd.Rank)
                                .Include(r => r.Contest)
                                .AsQueryable();

            // Áp dụng các bộ lọc nếu có
            if (!string.IsNullOrEmpty(filter.ownerName))
            {
                query = query.Where(r => r.RegisterDetail.Owner.Name.Contains(filter.ownerName));
            }

            if (!string.IsNullOrEmpty(filter.contestName))
            {
                query = query.Where(r => r.Contest.Name.Contains(filter.contestName));
            }

            if (!string.IsNullOrEmpty(filter.status))
            {
                query = query.Where(r => r.Status.Contains(filter.status));
            }

            if (!string.IsNullOrEmpty(filter.rankName))
            {
                query = query.Where(r => r.RegisterDetail.Rank.Name.Contains(filter.rankName));
            }

            if (filter.approvalDate.HasValue)
            {
                query = query.Where(r => r.ApprovalDate == filter.approvalDate);
            }

            if (!string.IsNullOrEmpty(filter.confirmationCode))
            {
                query = query.Where(r => r.ConfirmationCode.Contains(filter.confirmationCode));
            }

            // Đếm tổng số bản ghi thỏa mãn điều kiện
            var totalRecords = await query.CountAsync();

            // Áp dụng paging (Skip và Take)
            var registrations = await query.Skip((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

            // Trả về đối tượng PagedResult chứa dữ liệu phân trang
            var pageResult =  new PagedResult<Registration>
            {
                Items = registrations,
                TotalItems = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return pageResult;
        }

        public async Task<int> CountContestantsParticipatingByRankName(string rankName)
        {
            return await _context.Registrations
                .Include(r => r.RegisterDetail)
                .ThenInclude(rd => rd.Rank)
                .Include(r => r.Contest)
                .Where(r => r.RegisterDetail.Rank.Name.Equals(rankName) && r.Contest.Status.Equals("ACTIVE"))
                .CountAsync();
        }

        public async Task<int> CountAsync(SearchRegistrationFilter searchRegistrationFilter)
        {
            var query = _context.Registrations.AsQueryable();

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

            return await query.CountAsync();
        }
    }
}
