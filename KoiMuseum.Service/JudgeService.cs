using KoiMuseum.Common;
using KoiMuseum.Data;
using KoiMuseum.Data.Models;
using KoiMuseum.Service.Base;

namespace KoiMuseum.Service
{
    public interface IJudgeService
    {
        Task<IServiceResult> GetAll();
        Task<IServiceResult> GetById(int id);
        Task<IServiceResult> Save(Judge judge);
        Task<IServiceResult> DeleteById(int id);
    }

    public class JudgeService : IJudgeService
    {
        private readonly UnitOfWork _unitOfWork;

        public JudgeService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> DeleteById(int id)
        {
            try
            {
                var judgeById = await _unitOfWork.JudgeRepository.GetByIdAsync(id);

                if (judgeById == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new Judge());
                }
                else
                {
                    bool isDelete = await _unitOfWork.JudgeRepository.RemoveAsync(judgeById);
                    if (!isDelete)
                    {
                        return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, judgeById);
                    }
                    return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, judgeById);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IServiceResult> GetAll()
        {
            var judges = await _unitOfWork.JudgeRepository.GetAllAsync();

            if (judges == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, judges);
            }
        }

        public async Task<IServiceResult> GetById(int id)
        {
            var judgeById = await _unitOfWork.JudgeRepository.GetByIdAsync(id);

            if (judgeById == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, judgeById);
            }
        }

        public async Task<IServiceResult> Save(Judge judge)
        {
            try
            {
                int result = -1;

                if (judge != null && judge.Id <= 0)
                {
                    result = await _unitOfWork.JudgeRepository.CreateAsync(judge);
                    if (result > 0)
                    {
                        return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, judge);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                    }
                }
                else
                {
                    result = await _unitOfWork.JudgeRepository.UpdateAsync(judge);

                    if (result > 0)
                    {
                        return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, judge);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, judge);
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }
    }
}
