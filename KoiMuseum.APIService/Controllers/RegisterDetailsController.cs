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
        public async Task<IServiceResult> GetRegisterDetails()
        {
            return await _registerDetailService.GetAll();
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
        public async Task<IServiceResult> PutRegisterDetail(RegisterDetail registerDetail)
        {
            return await _registerDetailService.Save(registerDetail);
        }

        // POST: api/RegisterDetails
        [HttpPost]
        public async Task<IServiceResult> PostRegisterDetail(RegisterDetail registerDetail)
        {
            return await _registerDetailService.Save(registerDetail);
        }

        // DELETE: api/RegisterDetails/5
        [HttpDelete("{id}")]
        public async Task<IServiceResult> DeleteRegisterDetail(int id)
        {
            return await _registerDetailService.DeleteById(id);
        }
    }
}
