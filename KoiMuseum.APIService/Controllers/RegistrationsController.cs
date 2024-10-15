using KoiMuseum.Data.Filters;
using KoiMuseum.Data.Models;
using KoiMuseum.Service;
using KoiMuseum.Service.Base;
using Microsoft.AspNetCore.Mvc;

namespace KoiMuseum.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationsController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        // GET: api/Registrations
        [HttpGet]
        public async Task<IServiceResult> GetRegistrations()
        {
            return await _registrationService.GetAllV2();
        }

        [HttpGet("Registrationss")]
        public async Task<IServiceResult> SearchRegistrationsAsync([FromQuery] SearchRegistrationFilter searchRegistrationFilter)
        {
            return await _registrationService.SearchSortCombineDataRegistrationAndRegisterDetail(searchRegistrationFilter);
        }

        [HttpGet("counts")]
        public async Task<IServiceResult> CountContestantsParticipating([FromQuery] string rankName)
        {
            return await _registrationService.CountContestantsParticipating(rankName);
        }

        // GET: api/Registrations/5
        [HttpGet("{id}")]
        public async Task<IServiceResult> GetRegistration(int id)
        {
            var registration = await _registrationService.GetByIdV2(id);
            return registration;
        }

        [HttpGet("WithDetails/{id}")]
        public async Task<IServiceResult> GetByIdWithCombineRegisterDetailResponse(int id)
        {
            var registration = await _registrationService.GetByIdWithCombineRegisterDetailResponse(id);
            return registration;
        }

        // PUT: api/Registrations
        [HttpPut]
        public async Task<IServiceResult> PutRegistration(Registration registration)
        {
            return await _registrationService.Save(registration);
        }

        // POST: api/Registrations
        [HttpPost]
        public async Task<IServiceResult> PostRegistration(Registration registration)
        {
            return await _registrationService.Save(registration);
        }

        // DELETE: api/Registrations/5
        [HttpDelete("{id}")]
        public async Task<IServiceResult> DeleteRegistration(int id)
        {
            return await _registrationService.DeleteById(id);
        }

        // PUT: api/Registrations/ChangeStatus/5
        [HttpPut("ChangeStatus/{id}")]
        public async Task<IServiceResult> ChangeStatus(int id, string status)
        {
            return await _registrationService.ChangeStatus(id, status);
        }

    }
}
