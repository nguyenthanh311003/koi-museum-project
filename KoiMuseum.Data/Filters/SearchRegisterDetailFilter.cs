using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Filters
{
    public class SearchRegisterDetailFilter
    {
        public string? RankName { get; set; }
        public string? OwnerName { get; set; }
        public string? Gender { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
