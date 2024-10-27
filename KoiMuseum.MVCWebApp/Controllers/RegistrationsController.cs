using KoiMuseum.Common;
using KoiMuseum.Data.Dtos.Responses.Registration;
using KoiMuseum.Data.Models;
using KoiMuseum.Data.PagingModel;
using KoiMuseum.Service.Base;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KoiMuseum.MVCWebApp.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly Fa24Se172594Prn231G1KfsContext _context;

        public RegistrationsController(Fa24Se172594Prn231G1KfsContext context)
        {
            _context = context;
        }

        // GET: Registrations
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult KoiDetail()
        {
            return View();
        }

        public async Task<IActionResult> RegistrationsOfRank(string name, string contestName, string ownerName = "", int pageNumber = 1, int pageSize = 1)
        {
            using (var httpClient = new HttpClient())
            {
                var queryString = $"?contestName={Uri.EscapeDataString(contestName)}&rankName={Uri.EscapeDataString(name)}&ownerName={Uri.EscapeDataString(ownerName)}&pageNumber={pageNumber}&pageSize={pageSize}";

                var queryStringToCount = $"?rankName={Uri.EscapeDataString(name)}";

                // Fetch the paged registrations
                var response = await httpClient.GetAsync(Const.APIEndPoint + "Registrations/Registrationss" + queryString);

                // Fetch the total count of registrations
                var countResponse = await httpClient.GetAsync(Const.APIEndPoint + "Registrations/counts" + queryStringToCount);

                if (response.IsSuccessStatusCode && countResponse.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);

                    var countContent = await countResponse.Content.ReadAsStringAsync();
                    var countResult = JsonConvert.DeserializeObject<ServiceResult>(countContent);

                    if (result != null && result.Data != null)
                    {
                        var data = JsonConvert.DeserializeObject<RegistrationPagedResult>(result.Data.ToString());

                        if (data != null)
                        {
                            var totalItems = data.TotalItems;

                            var pagedResult = new PagedResult<RegistrationResponse>
                            {
                                Items = data.Items,
                                TotalItems = totalItems,
                                PageNumber = pageNumber,
                                PageSize = pageSize
                            };

                            ViewBag.PagedResult = pagedResult;
                            ViewBag.RegistrationCount = countResult.Data;
                            ViewBag.RankName = name;
                            ViewBag.ContestName = contestName;
                            ViewBag.OwnerName = ownerName;  // Thêm viewbag để lưu ownerName
                            return View(pagedResult);
                        }
                    }
                }

                ViewBag.PagedResult = new PagedResult<RegistrationResponse>
                {
                    Items = new List<RegistrationResponse>(),
                    TotalItems = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return View(new PagedResult<RegistrationResponse>());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        //public IActionResult Search(string searchString)
        //{
        // Store the current search string for use in the view
        //  ViewData["CurrentFilter"] = searchString;
        //public IActionResult Search(string searchString)
        //{
        //    // Store the current search string for use in the view
        //    ViewData["CurrentFilter"] = searchString;

        //    // Get all registrations
        //    var registrations = from r in _context.Registrations
        //                        select r;

        //    // Filter by search string
        //    //if (!String.IsNullOrEmpty(searchString))
        //    //{
        //    //    registrations = registrations.Where(r =>
        //    //        r..Contains(searchString) ||
        //    //        r.Rank.Contains(searchString) ||
        //    //        r.ContestName.Contains(searchString));
        //    //}

        //    // Return filtered results
        //    return View(registrations.ToList());
        //}

        // GET: Registrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View(id);
        }

        //GET: Registrations/Create
        /*public async Task<IActionResult> Create()
        {
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name");
            ViewData["RegisterDetailId"] = new SelectList(_context.RegisterDetails, "Id", "Id");
            return View();
        }

        //POST: Registrations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ContestId,RegisterDetailId,RegistrationDate,ApprovalDate,RejectedReason,ConfirmationCode,IntroductionOfOwner,IntroductionOfKoi,AdminReviewedBy,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(registration);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(Const.APIEndPoint + "Registrations", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name", registration.ContestId);
            ViewData["RegisterDetailId"] = new SelectList(_context.RegisterDetails, "Id", "Id", registration.RegisterDetailId);
            return View(registration);
        }*/

        //GET: Registrations/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    return View(id);
        //}

        //// POST: Registrations/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,ContestId,RegisterDetailId,RegistrationDate,ApprovalDate,RejectedReason,ConfirmationCode,IntroductionOfOwner,IntroductionOfKoi,AdminReviewedBy,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Registration registration)
        //{
        //    if (id != registration.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        using (var httpClient = new HttpClient())
        //        {
        //            var json = JsonConvert.SerializeObject(registration);
        //            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        //            var response = await httpClient.PutAsync(Const.APIEndPoint + $"Registrations/{id}", content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                return RedirectToAction(nameof(Index));
        //            }
        //        }
        //    }

        //    ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name", registration.ContestId);
        //    ViewData["RegisterDetailId"] = new SelectList(_context.RegisterDetails, "Id", "Id", registration.RegisterDetailId);
        //    return View(registration);
        //}

        //// GET: Registrations/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    using (var httpClient = new HttpClient())
        //    {
        //        var response = await httpClient.GetAsync(Const.APIEndPoint + $"Registrations/{id}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var content = await response.Content.ReadAsStringAsync();
        //            var result = JsonConvert.DeserializeObject<ServiceResult>(content);
        //            if (result != null && result.Data != null)
        //            {
        //                var registration = JsonConvert.DeserializeObject<Registration>(result.Data.ToString());
        //                return View(registration);
        //            }
        //        }
        //    }

        //    return NotFound();
        //}

        //// POST: Registrations/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        var response = await httpClient.DeleteAsync(Const.APIEndPoint + $"Registrations/{id}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }

        //    return RedirectToAction(nameof(Index));
        //}

        //private bool RegistrationExists(int id)
        //{
        //    return _context.Registrations.Any(e => e.Id == id);
        //}
        //// POST: Registrations/ChangeStatus/5
        //[HttpPost]
        //public async Task<IActionResult> ChangeStatus([FromBody] ChangeStatusRequest request)
        //{
        //    if (request == null || string.IsNullOrEmpty(request.Status))
        //    {
        //        return BadRequest("Status cannot be empty.");
        //    }

        //    // Prepare the HttpClient
        //    using (var httpClient = new HttpClient())
        //    {
        //        // Create the request body as a JSON object
        //        var json = JsonConvert.SerializeObject(new { id = request.Id, status = request.Status });
        //        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        //        // Send the POST request to change status
        //        var apiUrl = $"{Const.APIEndPoint}Registrations/ChangeStatus/{request.Id}?status={request.Status}";
        //        var response = await httpClient.PutAsync(apiUrl, content);

        //        // Check if the response is successful
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return Ok("Status changed successfully.");
        //        }

        //        // Return the error message if the API call failed
        //        var errorContent = await response.Content.ReadAsStringAsync();
        //        return BadRequest($"Error changing status: {errorContent}");
        //    }
        //}

        //// DTO for the incoming request
        //public class ChangeStatusRequest
        //{
        //    public int Id { get; set; }
        //    public string Status { get; set; }
        //}


    }
}

