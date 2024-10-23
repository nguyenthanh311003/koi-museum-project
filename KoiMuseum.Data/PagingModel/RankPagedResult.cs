using KoiMuseum.Data.Dtos.Responses.Ranks;
using KoiMuseum.Data.Dtos.Responses.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Data.PagingModel
{
    public class RankPagedResult
    {
        public List<RanksResponse> Items { get; set; } // Danh sách các đối tượng đăng ký
        public int TotalItems { get; set; } // Tổng số đối tượng đăng ký
        public int PageNumber { get; set; } // Số trang hiện tại
        public int PageSize { get; set; } // Số lượng đối tượng trên mỗi trang
        public int TotalPages { get; set; }
    }
}
