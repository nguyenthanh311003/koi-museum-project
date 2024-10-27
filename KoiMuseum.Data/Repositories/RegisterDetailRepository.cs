using KoiMuseum.Data.Base;
using KoiMuseum.Data.Filters;
using KoiMuseum.Data.Models;
using KoiMuseum.Data.PagingModel;
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

        public async Task<bool> DeleteAsyn(int registerDetailId)
        {
            var registerDetailById = await _context.RegisterDetails.SingleOrDefaultAsync(rd => rd.Id == registerDetailId);

            if (registerDetailById == null)
            {
                return false;
            }

            _context.Remove(registerDetailById);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PagedResult<RegisterDetail>> SearchRegisterDetailPagedAsync(SearchRegisterDetailFilter filter, int pageNumber, int pageSize)
        {
            var query = _context.RegisterDetails
                .Include(rd => rd.Rank)
                .Include(rd => rd.Owner)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.RankName))
            {
                query = query.Where(rd => rd.Rank.Name.Contains(filter.RankName));
            }

            if (!string.IsNullOrEmpty(filter.OwnerName))
            {
                query = query.Where(rd => rd.Owner.Name.Contains(filter.RankName));

            }

            if (!string.IsNullOrEmpty(filter.Gender))
            {
                query = query.Where(rd => rd.Gender.Contains(filter.Gender));
            }

            var totalRecords = await query.CountAsync();

            // Áp dụng paging (Skip và Take)
            var registerDetails = await query.Skip((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

            // Trả về đối tượng PagedResult chứa dữ liệu phân trang
            var pageResult = new PagedResult<RegisterDetail>
            {
                Items = registerDetails,
                TotalItems = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return pageResult;
        }
    }
}
