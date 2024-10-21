using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiMuseum.Data.Models;
using KoiMuseum.Common;
using Newtonsoft.Json;
using KoiMuseum.Service.Base;
using System.Text;

namespace KoiMuseum.MVCWebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly Fa24Se172594Prn231G1KfsContext _context;

        public UsersController(Fa24Se172594Prn231G1KfsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Const.APIEndPoint + "Users");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);
                    if (result != null && result.Data != null)
                    {
                        var data = JsonConvert.DeserializeObject<List<User>>(result.Data.ToString());
                        return View(data);
                    }
                }
            }

            return View(new List<User>());
        }

        // GET: Users
        /*[HttpGet]
        public async Task<JsonResult> GetUserss()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Const.APIEndPoint + "Users");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);
                    if (result != null && result.Data != null)
                    {
                        var data = JsonConvert.DeserializeObject<List<User>>(result.Data.ToString());
                        return Json(data);
                    }
                }
            }

            return Json(new List<User>());
        }*/

        public async Task<IActionResult> Thanh()
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Const.APIEndPoint + "Users"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServiceResult>(content);

                        if (result != null && result.Data != null)
                        {
                            var data = JsonConvert.DeserializeObject<List<User>>(result.Data.ToString());
                            return View(data);
                        }
                    }
                }
            }

            return View(new List<User>());
        }


        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password,Role,Address,PhoneNumber,AvatarUrl,Description,Status,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,Role,Address,PhoneNumber,AvatarUrl,Description,Status,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Login()
        {
            using (var httpClient = new HttpClient())
            {
                var user = new User();  // Assuming you want to return a new or existing user object
                return View(user);
            }
        }

        public async Task<IActionResult> Register()
        {
            using (var httpClient = new HttpClient())
            {
                var user = new User();  // Assuming you want to return a new or existing user object
                return View(user);
            }
        }

        /*[HttpPost]
        public async Task<IActionResult> LoginSubmit([FromBody] User loginModel)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var apiUrl = Const.APIEndPoint + "v1/login";  // Your API endpoint

                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        Email = loginModel.Email,
                        Password = loginModel.Password
                    }), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(apiUrl, content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var apiResponse = await response.Content.ReadAsStringAsync();

                            // Look for "success" keyword in the raw response
                            if (apiResponse.Contains("\"message\":\"success\""))
                            {
                                return Json(new { success = true, message = "Login successful", redirectUrl = Url.Action("Index", "Home") });
                            }
                            else
                            {
                                return Json(new { success = false, message = "Login failed." });
                            }
                        }
                        else
                        {
                            return Json(new { success = false, message = "Login failed. Please try again." });
                        }
                    }
                }
            }

            return Json(new { success = false, message = "Invalid login credentials." });
        }
    }*/
        [HttpPost]
        public IActionResult LoginSubmit([FromBody] User loginModel)
        {
            // Example logic for validating the login
            if (loginModel.Email == "test@example.com" && loginModel.Password == "password123")
            {
                return Json(new { success = true, message = "Login successful", redirectUrl = Url.Action("Index", "Home") });
            }
            else
            {
                return Json(new { success = false, message = "Invalid login credentials." });
            }
        }

    }
}
