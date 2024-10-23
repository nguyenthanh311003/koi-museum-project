using KoiMuseum.Common;
using KoiMuseum.Data;
using KoiMuseum.Data.Dtos.Responses.Ranks;
using KoiMuseum.Data.Filters;
using KoiMuseum.Data.PagingModel;
using KoiMuseum.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Service
{
    public interface IContestRankService
    {
        Task<IServiceResult> GetContestRankByContestIdAndRankId(int contestId, int rankId);

        Task<IServiceResult> GetAll(SearchRankFilter searchRankFilter);
    }
    public class ContestRankService : IContestRankService
    {
        private readonly UnitOfWork _unitOfWork;

        public ContestRankService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> GetContestRankByContestIdAndRankId(int contestId, int rankId)
        {
            try
            {
                var contestRankByContestIdAndRankId = await _unitOfWork.ContestRankRepository.GetContestRankByContestIdAndRankId(contestId, rankId);
                
                if (contestRankByContestIdAndRankId == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, contestRankByContestIdAndRankId);
            } catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IServiceResult> GetAll(SearchRankFilter searchRankFilter)
        {
            int pageNumber = searchRankFilter.PageNumber;
            int pageSize = 10;

            var getContestByStatus = await _unitOfWork.ContestRepository.GetContestByStatus("ACTIVE");

            if (getContestByStatus == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            var pagedResult = await _unitOfWork.ContestRankRepository.SearchContestRanksPagedAsync(searchRankFilter, pageNumber, pageSize, getContestByStatus.Id);

            if (pagedResult == null || !pagedResult.Items.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            var ranksResponses = new List<RanksResponse>();

            foreach (var rank in pagedResult.Items)
            {
                var countRegisterDetailByRankId = await _unitOfWork.RegisterDetailRepository.CountRegisterDetailByRankId(rank.RankId);
                    var rankResponse = new RanksResponse
                    {
                        Id = rank.RankId,
                        Name = rank.Rank.Name,
                        Participants = countRegisterDetailByRankId,
                        Description = rank.Rank.Description,
                        Criteria = rank.Rank.Criteria,
                        ContestName = getContestByStatus.Name,
                        Reward = (decimal)rank.Rank.Reward,
                        MinSize = rank.Rank.MinSize,
                        MaxSize = rank.Rank.MaxSize,
                        MinAge = rank.Rank.MinSize,
                        MaxAge = rank.Rank.MaxAge,
                        Status = rank.Status,
                        VarietyRestriction = rank.Rank.VarietyRestriction,
                    };
                    ranksResponses.Add(rankResponse);
            }

            var pagedResponse = new PagedResult<RanksResponse>
            {
                Items = ranksResponses,
                TotalItems = pagedResult.TotalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, pagedResponse);
        }
    }
}
