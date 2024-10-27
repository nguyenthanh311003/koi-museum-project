using KoiMuseum.Common;
using KoiMuseum.Data;
using KoiMuseum.Data.Dtos.Requests.User;
using KoiMuseum.Data.Models;
using KoiMuseum.Service.Base;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        Task<IServiceResult> GetByEmailAsync(string email);
        User GetByEmail(string email);
        Task<IServiceResult> GetJudgeUser(string queryString);
        Task<IServiceResult> DeleteJudgeUserById(int id);
        Task<IServiceResult> UpdateJudge(UpdateJudgeUserRequest judge);

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

        public async Task<IServiceResult> GetByEmailAsync(string email)
        {
            var userByEmail = _unitOfWork.UserRepository.GetByEmailAsync(email);
 
            if (userByEmail == null)
            {
                return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
            }
            else
            {
                return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, userByEmail);
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

        public User GetByEmail(string email)
        {
            var userByEmail = _unitOfWork.UserRepository.GetByEmail(email);

            if (userByEmail == null)
            {
                return null;
            }
            else
            {
                return userByEmail;
            }
        }

        public async Task<IServiceResult> GetJudgeUser(string queryString)
        {
            {
                var users = await _unitOfWork.UserRepository.GetJudgeUser(queryString);

                if (users == null)
                {
                    return new ServiceResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA_MSG);
                }
                else
                {
                    return new ServiceResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, users);
                }
            }
        }

        public async Task<IServiceResult> DeleteJudgeUserById(int id)
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
                    Boolean isJudgeDelete = await _unitOfWork.JudgeRepository.RemoveAsync(userById.Judges.FirstOrDefault());
                    if (!isJudgeDelete)
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

        public async Task<IServiceResult> UpdateJudge(UpdateJudgeUserRequest judge)
        {   try
            {
                var result = await _unitOfWork.UserRepository.UpdateJudge(judge);
                if (result)
                {
                    return new ServiceResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG, true);
                }
                else
                {
                    return new ServiceResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult(Const.ERROR_EXCEPTION, ex.ToString());
            }
           
        }
    }

    public class JwtService
    {
        private string secureKey = "this is a very secure key jwttttttttt";

        public string Generate(int id)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);

            var payload = new JwtPayload(id.ToString(), null, null, null, DateTime.Today.AddDays(1));
            var securityToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public JwtSecurityToken Verify(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secureKey);
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,

            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }
    }
}
