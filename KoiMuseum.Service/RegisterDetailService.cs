using KoiMuseum.Common;
using KoiMuseum.Data;
using KoiMuseum.Data.Dtos.Requests.CombineRegisterRequest;
using KoiMuseum.Data.Dtos.Requests.RegisterDetail;
using KoiMuseum.Data.Dtos.Responses.Ranks;
using KoiMuseum.Data.Dtos.Responses.RegisterDetails;
using KoiMuseum.Data.Filters;
using KoiMuseum.Data.Models;
using KoiMuseum.Data.PagingModel;
using KoiMuseum.Data.Repositories;
using KoiMuseum.Service.Base;
using System.Collections;
using System.Drawing;

namespace KoiMuseum.Service
{
    public interface IRegisterDetailService
    {
        Task<IServiceResult> GetAll();
        Task<IServiceResult> GetAllV2(SearchRegisterDetailFilter searchRegisterDetailFilter);
        Task<IServiceResult> GetById(int id);
        Task<IServiceResult> Save(RegisterDetail registerDetail);
        Task<IServiceResult> Create(CreateRegisterDetailAloneRq createRegisterDetailRequest);
        Task<IServiceResult> Update(UpdateRegisterDetailRequest updateRegisterDetailRequest);
        Task<IServiceResult> DeleteById(int id);
        Task<IServiceResult> CountParticipants(int rankId);
        Task<IServiceResult> GetsByRankId(int rankId);
        Task<IServiceResult> RegisterApplication(RegisterViewModel registerViewModel, int contestId, int userId);
    }

    public class RegisterDetailService : IRegisterDetailService
    {
        private readonly UnitOfWork _unitOfWork;

        public RegisterDetailService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> CountParticipants(int rankId)
        {
            try
            {
                var countRegisterDetailByRankId = await _unitOfWork.RegisterDetailRepository.CountRegisterDetailByRankId(rankId);

                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, countRegisterDetailByRankId);
            } catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IServiceResult> DeleteById(int id)
        {
            try
            {
                var registerDetailById = await _unitOfWork.RegisterDetailRepository.GetByIdAsync(id);
                if (registerDetailById == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new RegisterDetail());
                }

                bool isDelete = await _unitOfWork.RegisterDetailRepository.RemoveAsync(registerDetailById);
                if (!isDelete)
                {
                    return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, registerDetailById);
                }

                return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, registerDetailById);
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IServiceResult> GetAll()
        {
            var registerDetails = await _unitOfWork.RegisterDetailRepository.GetAllAsync();
            return registerDetails == null
                ? new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG)
                : new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, registerDetails);
        }

        public async Task<IServiceResult> GetAllV2(SearchRegisterDetailFilter searchRegisterDetailFilter)
        {
            int pageNumber = searchRegisterDetailFilter.PageNumber;
            int pageSize = 10;

            var pagedResult = await _unitOfWork.RegisterDetailRepository.SearchRegisterDetailPagedAsync(searchRegisterDetailFilter, pageNumber, pageSize);
            if (pagedResult == null || !pagedResult.Items.Any())
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }

            var registerResponse = new List<RegisterDetailResponse>();

            foreach (var item in pagedResult.Items)
            {
                RegisterDetailResponse registerDetailResponse = new RegisterDetailResponse();
                registerDetailResponse.Id = item.Id;
                registerDetailResponse.RankName = item.Rank.Name;
                registerDetailResponse.OwnerName = item.Owner.Name;
                registerDetailResponse.Size = item.Size;
                registerDetailResponse.Age = item.Age;
                registerDetailResponse.Type = item.Type;
                registerDetailResponse.Gender = item.Gender;
                registerDetailResponse.Status = item.Status;
                registerDetailResponse.ImageUrl = item.ImageUrl;
                registerDetailResponse.Name = item.Name;
                registerDetailResponse.Weight = item.Weight;

                registerResponse.Add(registerDetailResponse);
            }

            var pagedResponse = new PagedResult<RegisterDetailResponse>
            {
                Items = registerResponse,
                TotalItems = pagedResult.TotalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, pagedResponse);
        }

        public async Task<IServiceResult> GetById(int id)
        {
            var registerDetailById = await _unitOfWork.RegisterDetailRepository.GetByIdAsync(id);
            return registerDetailById == null
                ? new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG)
                : new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, registerDetailById);
        }

        public async Task<IServiceResult> GetsByRankId(int rankId)
        {
            try
            {
                var registerDetais = await _unitOfWork.RegisterDetailRepository.getRegisterDetailsByRankId(rankId);
                var registerDetailResponses = new List<GetRegisterDetailByRankIdResponse>();

                foreach (var item in registerDetais)
                {
                    var getRegisterDetailByRankIdResponse = new GetRegisterDetailByRankIdResponse
                    {
                        Id = item.Id,
                        RankId = (int) item.RankId,
                        OwnerId = (int) item.OwnerId,
                        Size = (int) item.Size,
                        Age = (int) item.Age,
                    };
                    registerDetailResponses.Add(getRegisterDetailByRankIdResponse);
                }

                return registerDetais == null || registerDetais.Count() == 0 
                    ? new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG)
                    : new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, registerDetailResponses);
            } catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IServiceResult> Save(RegisterDetail registerDetail)
        {
            try
            {
                int result = -1;

                if (registerDetail != null && registerDetail.Id <= 0)
                {
                    result = await _unitOfWork.RegisterDetailRepository.CreateAsync(registerDetail);
                    return result > 0
                        ? new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, registerDetail)
                        : new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
                else
                {
                    result = await _unitOfWork.RegisterDetailRepository.UpdateAsync(registerDetail);
                    return result > 0
                        ? new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, registerDetail)
                        : new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, registerDetail);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IServiceResult> Create(CreateRegisterDetailAloneRq createRegisterDetailRequest)
        {
            int result = -1;

            if (createRegisterDetailRequest != null) 
            {
                var rankById = await _unitOfWork.RankRepository.GetByIdAsync(createRegisterDetailRequest.RankId);

                if (rankById == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, "Rank not found");
                }

                var ownerById = await _unitOfWork.UserRepository.GetByIdAsync(createRegisterDetailRequest.OwnerId);

                if (ownerById == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, "Owner not found");
                }

                RegisterDetail newRegisterDetail = new RegisterDetail();
                newRegisterDetail.RankId = rankById.Id;
                newRegisterDetail.OwnerId = ownerById.Id;
                newRegisterDetail.Size = createRegisterDetailRequest.Size;
                newRegisterDetail.Age = createRegisterDetailRequest.Age;
                newRegisterDetail.Type = createRegisterDetailRequest.Type;
                newRegisterDetail.ImageUrl = createRegisterDetailRequest.ImageUrl;
                newRegisterDetail.Gender = createRegisterDetailRequest.Gender;
                newRegisterDetail.Name = createRegisterDetailRequest.Name;
                newRegisterDetail.Weight = createRegisterDetailRequest.Weight;

                result = await _unitOfWork.RegisterDetailRepository.CreateAsync(newRegisterDetail);

                if (result < 0)
                {
                    return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, newRegisterDetail);
                } else
                {
                    return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, newRegisterDetail);
                }
            }

            return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
        }

        public async Task<IServiceResult> Update(UpdateRegisterDetailRequest updateRegisterDetailRequest)
        {
            int result = -1;

            if (updateRegisterDetailRequest != null)
            {
                var rankById = await _unitOfWork.RankRepository.GetByIdAsync(updateRegisterDetailRequest.RankId);

                if (rankById == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, "Rank not found");
                }

                var ownerById = await _unitOfWork.UserRepository.GetByIdAsync(updateRegisterDetailRequest.OwnerId);

                if (ownerById == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, "Owner not found");
                }

                RegisterDetail newRegisterDetail = await _unitOfWork.RegisterDetailRepository.GetByIdAsync(updateRegisterDetailRequest.Id);
                newRegisterDetail.RankId = rankById.Id;
                newRegisterDetail.OwnerId = ownerById.Id;
                newRegisterDetail.Size = updateRegisterDetailRequest.Size;
                newRegisterDetail.Age = updateRegisterDetailRequest.Age;
                newRegisterDetail.Type = updateRegisterDetailRequest.Type;
                newRegisterDetail.ImageUrl = updateRegisterDetailRequest.ImageUrl;
                newRegisterDetail.Gender = updateRegisterDetailRequest.Gender;
                newRegisterDetail.Name = updateRegisterDetailRequest.Name;
                newRegisterDetail.Status = updateRegisterDetailRequest.Status;
                newRegisterDetail.Weight = updateRegisterDetailRequest.Weight;

                result = await _unitOfWork.RegisterDetailRepository.UpdateAsync(newRegisterDetail);

                if (result < 0)
                {
                    return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, newRegisterDetail);
                }
                else
                {
                    return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, newRegisterDetail);
                }
            }

            return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
        }

        public async Task<IServiceResult> RegisterApplication(RegisterViewModel registerViewModel, int contestId, int userId)
        {
            if (registerViewModel == null || contestId <= 0 || userId <=0)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, registerViewModel);
            }

            var contestById = await _unitOfWork.ContestRepository.GetByIdAsync(contestId);
            if (contestById == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, "Contest not found");
            }

            var userById = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (userById == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, "User not found");
            }

            var newRegisterDetail = new RegisterDetail
            {
                OwnerId = userById.Id,
                Size = registerViewModel.CreateRegisterDetailRequest.Size,
                Age = registerViewModel.CreateRegisterDetailRequest.Age,
                Type = registerViewModel.CreateRegisterDetailRequest.Type,
                Gender = registerViewModel.CreateRegisterDetailRequest.Gender,
                ImageUrl = registerViewModel.CreateRegisterDetailRequest.ImageUrl,
                Name = registerViewModel.CreateRegisterDetailRequest.Name,
                Weight = registerViewModel.CreateRegisterDetailRequest.Weight
            };

            int registerDetailResult = await _unitOfWork.RegisterDetailRepository.CreateAsync(newRegisterDetail);
            if (registerDetailResult <= 0)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, registerViewModel);
            }

            await ClassificationRank(newRegisterDetail.Id);

            var newRegistration = new Registration
            {
                ContestId = contestById.Id,
                RegisterDetailId = newRegisterDetail.Id,
                IntroductionOfOwner = registerViewModel.createRegistrationRequest.IntroductionOfOwner,
                IntroductionOfKoi = registerViewModel.createRegistrationRequest.IntroductionOfKoi
            };

            int registrationResult = await _unitOfWork.RegistrationRepository.CreateAsync(newRegistration);
            if (registrationResult <= 0)
            {
                // Xóa RegisterDetail nếu Registration không thành công
                await _unitOfWork.RegisterDetailRepository.DeleteAsyn(newRegisterDetail.Id);
                return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG, registerViewModel);
            }



            return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, registerViewModel);
        }

        private async Task<IServiceResult> ClassificationRank(int registerDetailId)
        {
            try
            {
                var registerDetailById = await _unitOfWork.RegisterDetailRepository.GetByIdAsync(registerDetailId);

                if (registerDetailById == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }

                // Khởi tạo Rank nếu chưa có
                if (registerDetailById.Rank == null)
                {
                    registerDetailById.Rank = new Rank(); // Khởi tạo một Rank mới
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
                    var rankToUpdate = await _unitOfWork.RankRepository.GetRankByNameAsync("Junior Rank");
                    registerDetailById.Rank = rankToUpdate;
                }
                else if (registerDetailById.Size >= 26 && registerDetailById.Size <= 50)
                {
                    var rankToUpdate = await _unitOfWork.RankRepository.GetRankByNameAsync("Senior Rank");
                    registerDetailById.Rank = rankToUpdate;
                }
                else if (registerDetailById.Size >= 51)
                {
                    var rankToUpdate = await _unitOfWork.RankRepository.GetRankByNameAsync("Master Rank");
                    registerDetailById.Rank = rankToUpdate;
                }
                else
                {
                    // Nếu kích cỡ không đủ để phân loại, tiếp tục phân loại theo cân nặng
                    if (double.TryParse(registerDetailById.Weight, out double weight))
                    {
                        if (weight >= 100 && weight <= 500)
                        {
                            var rankToUpdate = await _unitOfWork.RankRepository.GetRankByNameAsync("Junior Rank");
                            registerDetailById.Rank = rankToUpdate;
                        }
                        else if (weight > 500 && weight <= 2000)
                        {
                            var rankToUpdate = await _unitOfWork.RankRepository.GetRankByNameAsync("Senior Rank");
                            registerDetailById.Rank = rankToUpdate;
                        }
                        else if (weight > 2000)
                        {
                            var rankToUpdate = await _unitOfWork.RankRepository.GetRankByNameAsync("Master Rank");
                            registerDetailById.Rank = rankToUpdate;
                        }
                    }

                    // Nếu vẫn không đủ để phân loại, tiếp tục phân loại theo tuổi
                    if (registerDetailById.RankId == null) // Kiểm tra nếu Rank.Id vẫn chưa được gán
                    {
                        if (registerDetailById.Age < 1)
                        {
                            var rankToUpdate = await _unitOfWork.RankRepository.GetRankByNameAsync("Junior Rank");
                            registerDetailById.Rank = rankToUpdate;
                        }
                        else if (registerDetailById.Age >= 1 && registerDetailById.Age <= 3)
                        {
                            var rankToUpdate = await _unitOfWork.RankRepository.GetRankByNameAsync("Senior Rank");
                            registerDetailById.Rank = rankToUpdate;
                        }
                        else if (registerDetailById.Age > 3)
                        {
                            var rankToUpdate = await _unitOfWork.RankRepository.GetRankByNameAsync("Master Rank");
                            registerDetailById.Rank = rankToUpdate;
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
    }
}
