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
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.AspNetCore.Identity.Data;
using KoiMuseum.Data.Dtos.Requests.User;

namespace KoiMuseum.APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public UsersController(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
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

        // DELETE: api/Users/5
        [HttpDelete("Judge/{id}")]
        public async Task<IServiceResult> DeleteJudgeUser(int id)
        {
            return await _userService.DeleteJudgeUserById(id);
        }

        [HttpGet("JudgeUsers")]
        public async Task<IServiceResult> GetJudgeUser(string searchTerm = "")
        {
            return await _userService.GetJudgeUser(searchTerm);
        }

        [HttpPut("JudgeUpdate")]
        public async Task<IServiceResult> JudgeUpdate([FromForm] UpdateJudgeUserRequest judge)
        {
            return await _userService.UpdateJudge(judge);
        }

        //=================================================================================================//
        //=================================================================================================//
        //=================================================================================================//
        //=================================================================================================//

        //========================================AUTHENTICATION===========================================//

        //=================================================================================================//
        //=================================================================================================//
        //=================================================================================================//
        //=================================================================================================//

        [HttpGet("/api/v1/auth")]
        public IActionResult Hello()
        {
            return Ok("Success");
        }

        [HttpPost("/api/v1/register")]
        public async Task<IServiceResult> Register(CreateUserRequest createUserRequest)
        {
            var user = new User
            {
                Name = createUserRequest.Name,
                Email = createUserRequest.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password),
                Role = createUserRequest.Role,
                Status = "Active",
                Description = "Amateur Koi enthusiast",
                PhoneNumber = createUserRequest.PhoneNumber,
                Address = createUserRequest.Address,
                AvatarUrl = createUserRequest.AvatarUrl,
                CreatedBy = null,
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = DateTime.Now
            };

            return await _userService.Save(user);
        }

        [HttpPost("/api/v1/login")]
        public IActionResult Login(LoginRequest request)
        {
            var user = _userService.GetByEmail(request.Email);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid credentials" });
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid credentials" });
            }

            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions { HttpOnly = true });

            return Ok(new { message = "success" });
        }

        [HttpGet("/api/v1/get-logged-user")]
        public async Task<IServiceResult> User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verify(jwt);

                int id = int.Parse(token.Issuer);

                var user = await _userService.GetById(id);

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpPost("/api/v1/logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new { message = "success" });
        }

    }
}
