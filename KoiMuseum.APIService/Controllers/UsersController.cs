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

namespace KoiMuseum.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IServiceResult> GetUsers()
        {
            return await _userService.GetAll();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IServiceResult> GetUser(int id)
        {
            var user = await _userService.GetById(id);

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IServiceResult> PutUser(User user)
        {
            return await _userService.Save(user);
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IServiceResult> PostUser(User user)
        {
            return await _userService.Save(user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IServiceResult> DeleteUser(int id)
        {
            return await _userService.DeleteById(id);
        }

    }
}
