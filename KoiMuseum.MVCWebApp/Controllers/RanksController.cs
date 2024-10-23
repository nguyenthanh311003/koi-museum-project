using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiMuseum.Data.Models;
using KoiMuseum.Common;
using KoiMuseum.Service.Base;
using Newtonsoft.Json;
using KoiMuseum.Data.Dtos.Responses.Ranks;
using System.Drawing.Printing;
using KoiMuseum.Data.PagingModel;
using KoiMuseum.Data.Dtos.Responses.Registration;

namespace KoiMuseum.MVCWebApp.Controllers
{
    public class RanksController : Controller
    {
        private readonly Fa24Se172594Prn231G1KfsContext _context;

        public RanksController(Fa24Se172594Prn231G1KfsContext context)
        {
            _context = context;
        }

        // GET: Ranks
        public async Task<IActionResult> Index(string rankName = "", string reward = "", string status = "ACTIVE", int pageNumber = 1, int pageSize = 10)
        {
            using (var httpClient = new HttpClient())
            {
                var queryString = $"?RankName={Uri.EscapeDataString(rankName)}&Status={Uri.EscapeDataString(status)}&SortReward={Uri.EscapeDataString(reward)}&pageNumber={pageNumber}&pageSize={pageSize}";
                var response = await httpClient.GetAsync(Const.APIEndPoint + "Ranks" + queryString);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);
                    if (result != null && result.Data != null)
                    {
                        var data = JsonConvert.DeserializeObject<RankPagedResult>(result.Data.ToString());
                        if (data != null)
                        {
                            var totalItems = data.TotalItems;

                            var pagedResult = new PagedResult<RanksResponse>
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
                ViewBag.PagedResult = new PagedResult<RanksResponse>
                {
                    Items = new List<RanksResponse>(),
                    TotalItems = 0,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return View(new PagedResult<RanksResponse>());
            }
        }

        // GET: Ranks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(Const.APIEndPoint + "Ranks/" + id.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceResult>(content);
                    if (result != null && result.Data != null)
                    {
                        var data = JsonConvert.DeserializeObject<Rank>(result.Data.ToString());
                        return View(data);
                    }
                }
            }

            return View(new Rank());
        }

        // GET: Ranks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ranks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Criteria,Reward,Description,MinSize,MaxSize,MinAge,MaxAge,VarietyRestriction,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Rank rank)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rank);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rank);
        }

        // GET: Ranks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rank = await _context.Ranks.FindAsync(id);
            if (rank == null)
            {
                return NotFound();
            }
            return View(rank);
        }

        // POST: Ranks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Criteria,Reward,Description,MinSize,MaxSize,MinAge,MaxAge,VarietyRestriction,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Rank rank)
        {
            if (id != rank.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RankExists(rank.Id))
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
            return View(rank);
        }

        // GET: Ranks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rank = await _context.Ranks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rank == null)
            {
                return NotFound();
            }

            return View(rank);
        }

        // POST: Ranks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rank = await _context.Ranks.FindAsync(id);
            if (rank != null)
            {
                _context.Ranks.Remove(rank);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RankExists(int id)
        {
            return _context.Ranks.Any(e => e.Id == id);
        }
    }
}
