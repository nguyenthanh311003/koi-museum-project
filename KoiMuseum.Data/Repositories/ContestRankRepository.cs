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
    public class ContestRankRepository : GenericRepository<ContestRank>
    {
        public ContestRankRepository() { }

        public ContestRankRepository(Fa24Se172594Prn231G1KfsContext context) => _context = context;

        public async Task<ContestRank> GetContestRankByContestIdAndRankId(int contestId, int rankId)
        {
            return await _context.ContestRanks.FirstOrDefaultAsync(cr => cr.ContestId == contestId && cr.RankId == rankId);
        }

        public async Task<PagedResult<ContestRank>> SearchContestRanksPagedAsync(SearchRankFilter filter, int pageNumber, int pageSize, int contestId)
        {
            var query = _context.ContestRanks.Include(cr => cr.Rank).Include(cr => cr.Contest).AsQueryable();

            query = query.Where(cr => cr.Contest.Id == contestId);

            if (!string.IsNullOrEmpty(filter.RankName))
            {
                query = query.Where(cr => cr.Rank.Name.Contains(filter.RankName));
            }

            if (!string.IsNullOrEmpty(filter.Status) && !filter.Status.Equals("all"))
            {
                query = query.Where(cr => cr.Status.Contains(filter.Status));
            }

            if (!string.IsNullOrEmpty(filter.SortReward))
            {
                if (filter.SortReward.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(cr => cr.Rank.Reward);
                }
                else if (filter.SortReward.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(cr => cr.Rank.Reward);
                }
            }

            // Đếm tổng số bản ghi thỏa mãn điều kiện
            var totalRecords = await query.CountAsync();

            // Áp dụng paging (Skip và Take)
            var contestRanks = await query.Skip((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

            // Trả về đối tượng PagedResult chứa dữ liệu phân trang
            var pageResult = new PagedResult<ContestRank>
            {
                Items = contestRanks,
                TotalItems = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return pageResult;
        }

        public async Task<bool> DeleteRankWithDeleteContestRank(int rankId)
        {
            var contestRankById = await _context.ContestRanks.FirstOrDefaultAsync(r => r.RankId == rankId);

            if (contestRankById == null)
            {
                return false;
            } 

            _context.ContestRanks.Remove(contestRankById);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ContestRank>> GetContestRankByRankId(int rankId)
        {
            return await _context.ContestRanks.Where(cr => cr.RankId == rankId).ToListAsync();
        }
    }
}
