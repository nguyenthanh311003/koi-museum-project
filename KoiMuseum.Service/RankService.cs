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
        Task<IServiceResult> ClassificationRank(int registerDetailId);
    }

    public class RankService : IRankService
    {
        private readonly UnitOfWork _unitOfWork;

        public RankService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> ClassificationRank(int registerDetailId)
        {
            try
            {
                var registerDetailById = await _unitOfWork.RegisterDetailRepository.GetByIdAsync(registerDetailId);

                if (registerDetailById == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                // Phương thức này sẽ trả về Id của Rank dựa trên tên Rank
                async Task<int> GetRankIdByNameAsync(string rankName)
                {
                    var rank = await _unitOfWork.RankRepository.GetRankByNameAsync(rankName);
                    return rank?.Id ?? -1;
                }

                // Ưu tiên phân loại theo kích cỡ trước
                if (registerDetailById.Size >= 10 && registerDetailById.Size <= 25)
                {
                    registerDetailById.Rank.Id = await GetRankIdByNameAsync("Junior Rank");
                }
                else if (registerDetailById.Size >= 26 && registerDetailById.Size <= 50)
                {
                    registerDetailById.Rank.Id = await GetRankIdByNameAsync("Senior Rank");
                }
                else if (registerDetailById.Size >= 51)
                {
                    registerDetailById.Rank.Id = await GetRankIdByNameAsync("Master Rank");
                }
                else
                {
                    // Nếu kích cỡ không đủ để phân loại, tiếp tục phân loại theo cân nặng
                    if (double.TryParse(registerDetailById.Weight, out double weight))
                    {
                        if (weight >= 100 && weight <= 500)
                        {
                            registerDetailById.Rank.Id = await GetRankIdByNameAsync("Junior Rank");
                        }
                        else if (weight > 500 && weight <= 2000)
                        {
                            registerDetailById.Rank.Id = await GetRankIdByNameAsync("Senior Rank");
                        }
                        else if (weight > 2000)
                        {
                            registerDetailById.Rank.Id = await GetRankIdByNameAsync("Master Rank");
                        }
                    }

                    // Nếu vẫn không đủ để phân loại, tiếp tục phân loại theo tuổi
                    if (registerDetailById.Rank.Id == 0)
                    {
                        if (registerDetailById.Age < 1)
                        {
                            registerDetailById.Rank.Id = await GetRankIdByNameAsync("Junior Rank");
                        }
                        else if (registerDetailById.Age >= 1 && registerDetailById.Age <= 3)
                        {
                            registerDetailById.Rank.Id = await GetRankIdByNameAsync("Senior Rank");
                        }
                        else if (registerDetailById.Age > 3)
                        {
                            registerDetailById.Rank.Id = await GetRankIdByNameAsync("Master Rank");
                        }
                    }
                }

                // Cập nhật thông tin đăng ký chi tiết
                int result = await _unitOfWork.RegisterDetailRepository.UpdateAsync(registerDetailById);

                if (result > 0)
                {
                    return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, registerDetailById);
                }
                else
                {
                    return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, registerDetailById);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
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
