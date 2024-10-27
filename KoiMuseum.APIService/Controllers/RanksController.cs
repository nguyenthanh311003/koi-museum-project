using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KoiMuseum.Data.Models;
using KoiMuseum.Service;
using KoiMuseum.Service.Base;
using KoiMuseum.Data.Filters;
using KoiMuseum.Data.Dtos.Requests.Ranks;
using KoiMuseum.Common;

namespace KoiMuseum.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RanksController : ControllerBase
    {
        private readonly IRankService _rankService;
        private readonly IContestRankService _contestRankService;

        public RanksController(IRankService rankService, IContestRankService contestRankService)
        {
            _rankService = rankService;
            _contestRankService = contestRankService;
        }


        /*// GET: api/Ranks
        [HttpGet]
        public async Task<IServiceResult> GetRanks([FromQuery] SearchRankFilter searchRankFilter)
        {
            return await _rankService.GetAll(searchRankFilter);
        }*/

        [HttpGet]
        public async Task<IServiceResult> GetRanks([FromQuery] SearchRankFilter searchRankFilter)
        {
            return await _contestRankService.GetAll(searchRankFilter);
        }

        [HttpGet("GetAllActive")]
        public async Task<IServiceResult> GetAll()
        {
            return await _rankService.GetAllActive();
        }

        // GET: api/Ranks/5
        [HttpGet("{id}")]
        public async Task<IServiceResult> GetRank(int id)
        {
            return await _rankService.GetById(id);
        }

        [HttpPost]
        public async Task<IServiceResult> Create([FromBody] CreateRankRequest createRankRequest)
        {
            return await _rankService.Create(createRankRequest);
        }

        [HttpDelete("{rankId}")]
        public async Task<IServiceResult> Delete(int rankId)
        {
            return await _rankService.Delete(rankId);
        }

        [HttpPut("{rankId}")]
        public async Task<IServiceResult> Update(int rankId, [FromBody] UpdateRankRequest updateRankRequest)
        {
            return await _rankService.Update(rankId, updateRankRequest);
        }
    }
}
