using KoiMuseum.Common;
using KoiMuseum.Data;
using KoiMuseum.Data.Dtos.Responses.RegisterDetails;
using KoiMuseum.Data.Models;
using KoiMuseum.Data.Repositories;
using KoiMuseum.Service.Base;
using System.Collections;
using System.Drawing;

namespace KoiMuseum.Service
{
    public interface IRegisterDetailService
    {
        Task<IServiceResult> GetAll();
        Task<IServiceResult> GetById(int id);
        Task<IServiceResult> Save(RegisterDetail registerDetail);
        Task<IServiceResult> DeleteById(int id);

        Task<IServiceResult> CountParticipants(int rankId);
        Task<IServiceResult> GetsByRankId(int rankId);
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
    }
}
