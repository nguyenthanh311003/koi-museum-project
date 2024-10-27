using KoiMuseum.Common;
using KoiMuseum.Data.Dtos.Requests.CombineRegisterRequest;
using KoiMuseum.Data.Dtos.Requests.RegisterDetail;
using KoiMuseum.Data.Filters;
using KoiMuseum.Data.Models;
using KoiMuseum.Service;
using KoiMuseum.Service.Base;
using Microsoft.AspNetCore.Mvc;

namespace KoiMuseum.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterDetailsController : ControllerBase
    {
        private readonly IRegisterDetailService _registerDetailService;

        public RegisterDetailsController(IRegisterDetailService registerDetailService)
        {
            _registerDetailService = registerDetailService;
        }

        // GET: api/RegisterDetails
        [HttpGet]
        public async Task<IServiceResult> GetRanks([FromQuery] SearchRegisterDetailFilter searchRegisterDetailFilter)
        {
            return await _registerDetailService.GetAllV2(searchRegisterDetailFilter);
        }

        // GET: api/RegisterDetails/rank/5
        [HttpGet("rank/{rankId}")]
        public async Task<IServiceResult> GetRegisterDetailsByRankId(int rankId)
        {
            return await _registerDetailService.GetsByRankId(rankId);
        }

        // GET: api/RegisterDetails/5
        [HttpGet("{id}")]
        public async Task<IServiceResult> GetRegisterDetail(int id)
        {
            return await _registerDetailService.GetById(id);
        }

        // PUT: api/RegisterDetails
        [HttpPut]
        public async Task<IServiceResult> PutRegisterDetail([FromBody] UpdateRegisterDetailRequest updateRegisterDetailRequest)
        {
            return await _registerDetailService.Update(updateRegisterDetailRequest);
        }

        // POST: api/RegisterDetails
        [HttpPost]
        public async Task<IServiceResult> PostRegisterDetail([FromBody] CreateRegisterDetailAloneRq createRegisterDetailRequest)
        {
            return await _registerDetailService.Create(createRegisterDetailRequest);
        }

        // DELETE: api/RegisterDetails/5
        [HttpDelete("{id}")]
        public async Task<IServiceResult> DeleteRegisterDetail(int id)
        {
            return await _registerDetailService.DeleteById(id);
        }

        [HttpPost("registerApplication")]
        public async Task<IServiceResult> RegisterApplication([FromBody] RegisterViewModel registerViewModel, int contestId, int userId)
        {
            if (registerViewModel == null)
            {
                return new ServiceResult(Const.FAIL_CREATE_CODE, "Invalid input data.");
            }

            return await _registerDetailService.RegisterApplication(registerViewModel, contestId, userId);
        }
    }
}
