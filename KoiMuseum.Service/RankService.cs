using KoiMuseum.Common;
using KoiMuseum.Data;
using KoiMuseum.Data.Dtos.Responses.Ranks;
using KoiMuseum.Data.Models;
using KoiMuseum.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Service
{
    public interface IRankService
    {
        Task<IServiceResult> GetAll();
        Task<IServiceResult> GetById(int id);
    }

    public class RankService : IRankService
    {
        private readonly UnitOfWork _unitOfWork;

        public RankService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> GetAll()
        {
            var ranks = await _unitOfWork.RankRepository.GetAllAsync();

            if (ranks == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            List<RanksResponse> ranksResponses = ranks.Select(rank => new RanksResponse
            {
                Id = rank.Id,
                Name = rank.Name,
                Description = rank.Description,
                Criteria = rank.Criteria,
                Reward = (decimal) rank.Reward,
                MinSize = rank.MinSize,
                MaxSize = rank.MaxSize,
                MinAge = rank.MinAge,
                MaxAge = rank.MaxAge,
                VarietyRestriction = rank.VarietyRestriction,
            }).ToList();

            return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, ranksResponses);
        }

        public async Task<IServiceResult> GetById(int id)
        {
            var rankById = await _unitOfWork.RankRepository.GetByIdAsync(id);

            if (rankById == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, rankById);
            }
        }
    }
}
