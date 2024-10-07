using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.Dtos.Responses.RegisterDetails
{
    public class GetRegisterDetailByRankIdResponse
    {
        public int Id { get; set; }

        public int RankId { get; set; }

        public int OwnerId { get; set; }

        public decimal Size { get; set; }

        public int Age { get; set; }

        public string ColorPattern { get; set; }
    }
}
