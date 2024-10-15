using KoiMuseum.Common;
using KoiMuseum.Data;
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
    }
}
