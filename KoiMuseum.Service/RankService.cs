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

            var ranksResponses = new List<RanksResponse>();

            var getContestByStatus = await _unitOfWork.ContestRepository.GetContestByStatus("ACTIVE");

            if (getContestByStatus == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            foreach (var rank in ranks)
            {
                var countRegisterDetailByRankId = await _unitOfWork.RegisterDetailRepository.CountRegisterDetailByRankId(rank.Id);

                var getContestRank = await _unitOfWork.ContestRankRepository.GetContestRankByContestIdAndRankId(getContestByStatus.Id, rank.Id);

                if (getContestRank == null)
                {
                    continue;
                } else
                {
                    var rankResponse = new RanksResponse
                    {
                        Id = rank.Id,
                        Name = rank.Name,
                        Participants = countRegisterDetailByRankId,
                        Description = rank.Description,
                        Criteria = rank.Criteria,
                        ContestName = getContestByStatus.Name,
                        Reward = (decimal)rank.Reward,
                        MinSize = rank.MinSize,
                        MaxSize = rank.MaxSize,
                        MinAge = rank.MinAge,
                        MaxAge = rank.MaxAge,
                        Status = getContestRank.Status,
                        VarietyRestriction = rank.VarietyRestriction,
                    };
                    ranksResponses.Add(rankResponse);
                } 
            }

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
