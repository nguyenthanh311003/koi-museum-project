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
using KoiMuseum.Data.PagingModel;
using KoiMuseum.Data.Dtos.Requests.User;
using KoiMuseum.Data.Dtos.Responses.Judge;
using KoiMuseum.Data.Repositories;

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

        public async Task<IActionResult> DeleteJudge(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var judge = await _context.Judges.FirstOrDefaultAsync(x => x.Id == id);
            if (judge == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .Where(u => u.Role == "Judge")
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.Id} - {u.Name}"
                }).ToListAsync();

            var assignedContests = await _context.Contests
                .Select(c => new SelectListItem
                {
                    Value = c.Name,
                    Text = c.Name
                }).ToListAsync();

            ViewBag.UserList = users;
            ViewBag.ContestList = assignedContests;

            return View(judge);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("DeleteJudgeConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJudgeConfirmed(int id)
        {
            var judge = await _context.Judges.FindAsync(id);
            if (judge != null)
            {
                _context.Judges.Remove(judge);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminJudgeList));
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


        [HttpGet]
        [Route("Users/AdminJudgeList")]
        public async Task<IActionResult> AdminJudgeList(string searchTerm = "", int pageNumber = 1, int pageSize = 10)
        {
            using (var httpClient = new HttpClient())
            {
                // Construct query parameters for pagination and search
                var queryString = $"?pageNumber={pageNumber}&pageSize={pageSize}&searchTerm={searchTerm}";

                // Make API request to fetch judge users with search functionality
                var response = await httpClient.GetAsync(Const.APIEndPoint + "Users/JudgeUsers" + queryString);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);

                    if (result != null && result.Data != null)
                    {
                        var data = JsonConvert.DeserializeObject<List<JudgeUserResponse>>(result.Data.ToString());

                        // Prepare paged result for the view
                        var pagedResult = new PagedResult<JudgeUserResponse>
                        {
                            Items = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                            TotalItems = data.Count,
                            PageNumber = pageNumber,
                            PageSize = pageSize
                        };

                        // Pass the paged result and search term to the view
                        ViewBag.SearchTerm = searchTerm;
                        ViewBag.PagedResult = pagedResult;
                        return View(pagedResult);
                    }
                }

                // Return empty result if API call fails
                ViewBag.PagedResult = new PagedResult<JudgeUserResponse>
                {
                    Items = new List<JudgeUserResponse>(),
                    TotalItems = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return View(new PagedResult<JudgeUserResponse>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJudge([Bind("UserId,Experience,Certifications")] Judge judge)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentContest = await _context.Contests.FirstOrDefaultAsync(x => x.Status.Equals("ACTIVE"));
                    if (currentContest == null)
                    {
                        ModelState.AddModelError("", "No active contest found.");
                        return View(judge);
                    }

                    // Check if there's already a Judge assigned to the current contest
                    var existingJudge = await _context.Judges
                        .FirstOrDefaultAsync(j => j.UserId == judge.UserId && j.AssignedContests == currentContest.Name);

                    if (existingJudge != null)
                    {
                        ModelState.AddModelError("", "A judge for the current contest already exists.");
                        ViewData["Users"] = new SelectList(await _context.Users.Where(u => u.Role == "Judge").ToListAsync(), "Id", "Name");
                        return View(judge);
                    }

                    var newJudge = new Judge
                    {
                        Experience = judge.Experience,
                        Certifications = judge.Certifications,
                        Status = "ACTIVE",  // Hardcoding Status to "ACTIVE"
                        UserId = judge.UserId,
                        AssignedContests = currentContest.Name,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        CreatedBy = 1,
                        UpdatedBy = 1
                    };

                    _context.Judges.Add(newJudge);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AdminJudgeList"); // Redirect to the list of judges or another appropriate page
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }

            // Load Users list for dropdown in case of failure
            ViewData["Users"] = new SelectList(await _context.Users.Where(u => u.Role == "Judge").ToListAsync(), "Id", "Name");
            return View(judge);
        }

        public async Task<IActionResult> CreateJudge()
        {
            ViewData["Users"] = new SelectList(await _context.Users.Where(u => u.Role == "Judge").ToListAsync(), "Id", "Name");
            return View();
        }

        public async Task<IActionResult> UpdateJudge(int id)
        {
            var judge = await _context.Judges.FirstOrDefaultAsync(x => x.Id == id);
            if (judge == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .Where(u => u.Role == "Judge")
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = $"{u.Id} - {u.Name}"
                }).ToListAsync();

            var assignedContests = await _context.Contests
                .Select(c => new SelectListItem
                {
                    Value = c.Name,
                    Text = c.Name
                }).ToListAsync();

            ViewBag.UserList = users;
            ViewBag.ContestList = assignedContests;

            return View(judge);
        }
    }
    }
