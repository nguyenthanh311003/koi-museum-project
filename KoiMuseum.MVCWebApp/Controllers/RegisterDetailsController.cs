using KoiMuseum.Common;
using KoiMuseum.Data.Dtos.Responses.Ranks;
using KoiMuseum.Data.Dtos.Responses.RegisterDetails;
using KoiMuseum.Data.Dtos.Responses.Registration;
using KoiMuseum.Data.Models;
using KoiMuseum.Data.PagingModel;
using KoiMuseum.Service.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Newtonsoft.Json;

namespace KoiMuseum.MVCWebApp.Controllers
{
    public class RegisterDetailsController : Controller
    {
        private readonly Fa24Se172594Prn231G1KfsContext _context;

        public RegisterDetailsController(Fa24Se172594Prn231G1KfsContext context)
        {
            _context = context;
        }

        // GET: RegisterDetails
        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Const.APIEndPoint + "RegisterDetails");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);
                    if (result != null && result.Data != null)
                    {
                        var data = JsonConvert.DeserializeObject<List<RegistrationResponse>>(result.Data.ToString());
                        return View(data);
                    }
                }
            }

            return View(new List<RegistrationResponse>());
        }

        public IActionResult CreateRegisterDetail()
        {
            return View();
        }

        public async Task<IActionResult> RegisterDetailList(string rankName = "", string ownerName = "", string gender = "", int pageNumber = 1, int pageSize = 10)
        {
            using (var httpClient = new HttpClient())
            {
                var queryString = $"?RankName={Uri.EscapeDataString(rankName)}&OwnerName={Uri.EscapeDataString(ownerName)}&Gender={Uri.EscapeDataString(gender)}&pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await httpClient.GetAsync(Const.APIEndPoint + "RegisterDetails" + queryString);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);
                    if (result != null && result.Data != null)
                    {
                        var data = JsonConvert.DeserializeObject<RegisterDetailPagedResult>(result.Data.ToString());
                        if (data != null)
                        {
                            var totalItems = data.TotalItems;

                            var pagedResult = new PagedResult<RegisterDetailResponse>
                            {
                                Items = data.Items,
                                TotalItems = totalItems,
                                PageNumber = pageNumber,
                                PageSize = pageSize
                            };

                            ViewBag.PagedResult = pagedResult;
                            return View(pagedResult);
                        }
                    }
                }
                ViewBag.PagedResult = new PagedResult<RegisterDetailResponse>
                {
                    Items = new List<RegisterDetailResponse>(),
                    TotalItems = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return View(new PagedResult<RegisterDetailResponse>());
            }
        }

        // GET: RegisterDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Const.APIEndPoint + $"RegisterDetails/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);
                    if (result != null && result.Data != null)
                    {
                        var registerDetail = JsonConvert.DeserializeObject<RegisterDetail>(result.Data.ToString());
                        return View(registerDetail);
                    }
                }
            }

            return NotFound();
        }

        // GET: RegisterDetails/Create
        public async Task<IActionResult> Create()
        {
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["RankId"] = new SelectList(_context.Ranks, "Id", "Name");
            return View();
        }

        // POST: RegisterDetails/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RankId,OwnerId,Size,Age,ColorPattern,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] RegisterDetail registerDetail)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(registerDetail);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(Const.APIEndPoint + "RegisterDetails", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Email", registerDetail.OwnerId);
            ViewData["RankId"] = new SelectList(_context.Ranks, "Id", "Name", registerDetail.RankId);
            return View(registerDetail);
        }

        // GET: RegisterDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Const.APIEndPoint + $"RegisterDetails/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);
                    if (result != null && result.Data != null)
                    {
                        var registerDetail = JsonConvert.DeserializeObject<RegisterDetail>(result.Data.ToString());
                        ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Email", registerDetail.OwnerId);
                        ViewData["RankId"] = new SelectList(_context.Ranks, "Id", "Name", registerDetail.RankId);
                        return View(registerDetail);
                    }
                }
            }

            return NotFound();
        }

        // POST: RegisterDetails/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RankId,OwnerId,Size,Age,ColorPattern,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] RegisterDetail registerDetail)
        {
            if (id != registerDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(registerDetail);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = await httpClient.PutAsync(Const.APIEndPoint + $"RegisterDetails/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Email", registerDetail.OwnerId);
            ViewData["RankId"] = new SelectList(_context.Ranks, "Id", "Name", registerDetail.RankId);
            return View(registerDetail);
        }

        // GET: RegisterDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Const.APIEndPoint + $"RegisterDetails/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);
                    if (result != null && result.Data != null)
                    {
                        var registerDetail = JsonConvert.DeserializeObject<RegisterDetail>(result.Data.ToString());
                        return View(registerDetail);
                    }
                }
            }

            return NotFound();
        }

        // POST: RegisterDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync(Const.APIEndPoint + $"RegisterDetails/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RegisterDetailExists(int id)
        {
            return _context.RegisterDetails.Any(e => e.Id == id);
        }
    }
}
