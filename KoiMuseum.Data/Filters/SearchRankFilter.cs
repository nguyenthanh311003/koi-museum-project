using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Filters
{
    public class SearchRankFilter
    {
        public string? RankName { get; set; }
        public string? Status { get; set; }
        public string? SortReward { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
