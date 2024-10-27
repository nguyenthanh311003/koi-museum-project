using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Dtos.Requests.RegisterDetail
{
    public class CreateRegisterDetailAloneRq
    {
        public int RankId { get; set; }
        public int OwnerId { get; set; }
        public decimal? Size { get; set; }

        public int? Age { get; set; }

        public string? Type { get; set; }

        public string? Gender { get; set; }

        public string? ImageUrl { get; set; }

        public string? Name { get; set; }

        public string? Weight { get; set; }
    }
}
