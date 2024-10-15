using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Filters
{
    public class SearchRegistrationFilter
    {
        public string? ownerName { get; set; }

        public string? contestName { get; set; }

        public string? status { get; set; }

        public string? rankName { get; set; }

        public string? colorPattern { get; set; }

        public DateOnly? approvalDate { get; set; }

        public string? confirmationCode { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }
}
