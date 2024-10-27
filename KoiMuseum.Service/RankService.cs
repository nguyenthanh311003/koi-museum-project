using KoiMuseum.Common;
using KoiMuseum.Data;
using KoiMuseum.Data.Dtos.Requests.Ranks;
using KoiMuseum.Data.Dtos.Responses.Ranks;
using KoiMuseum.Data.Dtos.Responses.Registration;
using KoiMuseum.Data.Filters;
using KoiMuseum.Data.Models;
using KoiMuseum.Data.PagingModel;
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
        Task<IServiceResult> GetAll(SearchRankFilter searchRankFilter);
        Task<IServiceResult> GetAllActive();
        Task<IServiceResult> Create(CreateRankRequest createRankRequest);
        Task<IServiceResult> GetById(int id);
        Task<IServiceResult> ClassificationRank(int registerDetailId);
        public Task<IServiceResult> Delete(int rankId);
        public Task<IServiceResult> Update(int rankId, UpdateRankRequest updateRankRequest);
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

        public async Task<IServiceResult> Create(CreateRankRequest createRankRequest)
        {
            try
            {
                int result = -1;
                int createContestRankResult = -1;
                if (createRankRequest != null)
                {
                    var newRank = new Rank();
                    newRank.Name = createRankRequest.Name;
                    newRank.Description = createRankRequest.Description;
                    newRank.MinSize = createRankRequest.MinSize;
                    newRank.MaxSize = createRankRequest.MaxSize;
                    newRank.Criteria = createRankRequest.Criteria;
                    newRank.Reward = createRankRequest.Reward;
                    newRank.MinAge = createRankRequest.MinAge;
                    newRank.MaxAge = createRankRequest.MaxAge;
                    newRank.VarietyRestriction = createRankRequest.VarietyRestriction;
                    newRank.CreatedDate = DateTime.Now;
                    result = await _unitOfWork.RankRepository.CreateAsync(newRank);
                    var contest = await _unitOfWork.ContestRepository.GetContestByStatus("ACTIVE");
                    var contestRank = new ContestRank();
                    contestRank.RankId = newRank.Id;
                    contestRank.ContestId = contest.Id;
                    contestRank.Status = "Active";
                    createContestRankResult = await _unitOfWork.ContestRankRepository.CreateAsync(contestRank);
                    if (result > 0 && createContestRankResult > 0)
                    {
                        return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, newRank);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                    }
                }
            } catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }

            return new ServiceResult(Const.FAIL_CREATE_CODE, "Invalid request");
        }

        public async Task<IServiceResult> GetAll(SearchRankFilter searchRankFilter)
        {
            int pageNumber = searchRankFilter.PageNumber;
            int pageSize = 1;

            /*var ranks = await _unitOfWork.RankRepository.GetAllAsync();*/

            var pagedResult = await _unitOfWork.RankRepository.SearchRanksPagedAsync(searchRankFilter, pageNumber, pageSize);

            if (pagedResult == null || !pagedResult.Items.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            var ranksResponses = new List<RanksResponse>();

            var getContestByStatus = await _unitOfWork.ContestRepository.GetContestByStatus("ACTIVE");

            if (getContestByStatus == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            foreach (var rank in pagedResult.Items)
            {
                var countRegisterDetailByRankId = await _unitOfWork.RegisterDetailRepository.CountRegisterDetailByRankId(rank.Id);

                var getContestRank = await _unitOfWork.ContestRankRepository.GetContestRankByContestIdAndRankId(getContestByStatus.Id, rank.Id);

                if (getContestRank == null)
                {
                    continue;
                }
                else
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

            var pagedResponse = new PagedResult<RanksResponse>
            {
                Items = ranksResponses,
                TotalItems = pagedResult.TotalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, pagedResponse);
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

        public async Task<IServiceResult> Delete(int rankId)
        {
            try
            {
                // Lấy thông tin rank theo ID
                var rankById = await _unitOfWork.RankRepository.GetByIdAsync(rankId);

                // Kiểm tra xem rank có tồn tại không
                if (rankById == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, null);
                }

                // Lấy danh sách ContestRank liên quan
                List<ContestRank> contestRanks = await _unitOfWork.ContestRankRepository.GetContestRankByRankId(rankId);

                // Xóa ContestRank liên quan
                if (contestRanks.Any())
                {
                    foreach (var contestRank in contestRanks)
                    {
                        await _unitOfWork.ContestRankRepository.DeleteRankWithDeleteContestRank(rankId); // Sửa lại để sử dụng contestRank.Id
                    }
                }

                bool isDelete = await _unitOfWork.RankRepository.RemoveAsync(rankById);
                if (!isDelete)
                {
                    return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, rankById);
                }

                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, rankById);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IServiceResult> Update(int rankId, UpdateRankRequest updateRankRequest)
        {
            int result = -1;

            var rankById = await _unitOfWork.RankRepository.GetByIdAsync(rankId);

            if (rankById == null || updateRankRequest == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Rank());               
            }

            rankById.Name = updateRankRequest.Name;
            rankById.Description = updateRankRequest.Description;
            rankById.MinSize = updateRankRequest.MinSize;
            rankById.MaxSize = updateRankRequest.MaxSize;
            rankById.Criteria = updateRankRequest.Criteria;
            rankById.Reward = updateRankRequest.Reward;
            rankById.MinAge = updateRankRequest.MinAge;
            rankById.MaxAge = updateRankRequest.MaxAge;
            rankById.VarietyRestriction = updateRankRequest.VarietyRestriction;
            rankById.CreatedDate = DateTime.Now;

            result = await _unitOfWork.RankRepository.UpdateAsync(rankById);

            if (result > 0)
            {
                return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, rankById);
            }
            else
            {
                return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, rankById);
            }
        }

        public async Task<IServiceResult> GetAllActive()
        {
            var ranks = await _unitOfWork.RankRepository.GetAllAsync();

            if (ranks.Count < 0)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, ranks);
            }

            return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, ranks);
        }
    }
}
