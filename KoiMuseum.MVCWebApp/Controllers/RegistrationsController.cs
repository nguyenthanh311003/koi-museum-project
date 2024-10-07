using KoiMuseum.Common;
using KoiMuseum.Data.Dtos.Responses.Registration;
using KoiMuseum.Data.Models;
using KoiMuseum.Service.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Const.APIEndPoint + "Registrations");
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
        public IActionResult Search(string searchString)
        {
            // Store the current search string for use in the view
            ViewData["CurrentFilter"] = searchString;

            // Get all registrations
            var registrations = from r in _context.Registrations
                                select r;

            // Filter by search string
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    registrations = registrations.Where(r =>
            //        r..Contains(searchString) ||
            //        r.Rank.Contains(searchString) ||
            //        r.ContestName.Contains(searchString));
            //}

            // Return filtered results
            return View(registrations.ToList());
        }

        // GET: Registrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Const.APIEndPoint + $"Registrations/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);
                    if (result != null && result.Data != null)
                    {
                        var registration = JsonConvert.DeserializeObject<RegistrationResponse>(result.Data.ToString());
                        return View(registration);
                    }
                }
            }

            return NotFound();
        }

        // GET: Registrations/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name");
            ViewData["RegisterDetailId"] = new SelectList(_context.RegisterDetails, "Id", "Id");
            return View();
        }

        // POST: Registrations/Create
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
        }

        // GET: Registrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Const.APIEndPoint + $"Registrations/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);
                    if (result != null && result.Data != null)
                    {
                        var registration = JsonConvert.DeserializeObject<Registration>(result.Data.ToString());
                        ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name", registration.ContestId);
                        ViewData["RegisterDetailId"] = new SelectList(_context.RegisterDetails, "Id", "Id", registration.RegisterDetailId);
                        return View(registration);
                    }
                }
            }

            return NotFound();
        }

        // POST: Registrations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ContestId,RegisterDetailId,RegistrationDate,ApprovalDate,RejectedReason,ConfirmationCode,IntroductionOfOwner,IntroductionOfKoi,AdminReviewedBy,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Registration registration)
        {
            if (id != registration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(registration);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = await httpClient.PutAsync(Const.APIEndPoint + $"Registrations/{id}", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name", registration.ContestId);
            ViewData["RegisterDetailId"] = new SelectList(_context.RegisterDetails, "Id", "Id", registration.RegisterDetailId);
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Const.APIEndPoint + $"Registrations/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);
                    if (result != null && result.Data != null)
                    {
                        var registration = JsonConvert.DeserializeObject<Registration>(result.Data.ToString());
                        return View(registration);
                    }
                }
            }

            return NotFound();
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync(Const.APIEndPoint + $"Registrations/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationExists(int id)
        {
            return _context.Registrations.Any(e => e.Id == id);
        }
        // POST: Registrations/ChangeStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Status cannot be empty.");
            }

            // Prepare the HttpClient
            using (var httpClient = new HttpClient())
            {
                // Create the request body as a JSON object
                var json = JsonConvert.SerializeObject(new { id = id, status = status });
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Send the POST request to change status
                var response = await httpClient.PostAsync(Const.APIEndPoint + $"Registrations/ChangeStatus/", content);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    return Ok("Status changed successfully.");
                }

                // Return the error message if the API call failed
                var errorContent = await response.Content.ReadAsStringAsync();
                return BadRequest($"Error changing status: {errorContent}");
            }
        }


    }
}
