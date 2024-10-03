using KoiMuseum.Common;
using KoiMuseum.Data;
using KoiMuseum.Data.Models;
using KoiMuseum.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiMuseum.Service
{
    public interface IUserService
    {
        Task<IServiceResult> GetAll();
        Task<IServiceResult> GetById(int id);
        Task<IServiceResult> Save(User user);
        Task<IServiceResult> DeleteById(int id);
    }

    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;

        public UserService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IServiceResult> DeleteById(int id)
        {
            try
            {
                var userById = await _unitOfWork.UserRepository.GetByIdAsync(id);

                if (userById == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG, new User());
                }
                else
                {
                    Boolean isDelete = await _unitOfWork.UserRepository.RemoveAsync(userById);
                    if (!isDelete)
                    {
                        return new ServiceResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG, userById);
                    }
                    return new ServiceResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG, userById);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
        }

        public async Task<IServiceResult> GetAll()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();

            if (users == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, users);
            }
        }

        public async Task<IServiceResult> GetById(int id)
        {
            var userById = await _unitOfWork.UserRepository.GetByIdAsync(id);

            if (userById == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, userById);
            }
        }

        public async Task<IServiceResult> Save(User user)
        {
            try
            {
                int result = -1;

                if (user != null && user.Id <= 0)
                {
                    result = await _unitOfWork.UserRepository.CreateAsync(user);
                    if (result > 0)
                    {
                        return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, user);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                    }
                }
                else
                {
                    result = await _unitOfWork.UserRepository.UpdateAsync(user);

                    if (result > 0)
                    {
                        return new ServiceResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG, user);
                    }
                    else
                    {
                        return new ServiceResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG, user);
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
