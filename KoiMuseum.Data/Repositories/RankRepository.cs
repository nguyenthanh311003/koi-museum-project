using KoiMuseum.Data.Base;
using KoiMuseum.Data.Filters;
using KoiMuseum.Data.Models;
using KoiMuseum.Data.PagingModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Repositories
{
    public class RankRepository : GenericRepository<Rank>
    {
        public RankRepository() { }

        public RankRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;

        public async Task<Rank> GetRankByNameAsync(string rankName)
        {
            return _context.Ranks.FirstOrDefault(r => r.Name.Equals(rankName));
        }

        public async Task<PagedResult<Rank>> SearchRanksPagedAsync(SearchRankFilter filter, int pageNumber, int pageSize)
        {
            var query = _context.Ranks.AsQueryable();

            // Áp dụng các bộ lọc nếu có
            if (!string.IsNullOrEmpty(filter.RankName))
            {
                query = query.Where(r => r.Name.Contains(filter.RankName));
            }

            if (!string.IsNullOrEmpty(filter.SortReward))
            {
                if (filter.SortReward.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(r => r.Reward);
                }
                else if (filter.SortReward.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(r => r.Reward);
                }
            }

            // Đếm tổng số bản ghi thỏa mãn điều kiện
            var totalRecords = await query.CountAsync();

            // Áp dụng paging (Skip và Take)
            var ranks = await query.Skip((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

            // Trả về đối tượng PagedResult chứa dữ liệu phân trang
            var pageResult = new PagedResult<Rank>
            {
                Items = ranks,
                TotalItems = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return pageResult;
        }
    }
}
