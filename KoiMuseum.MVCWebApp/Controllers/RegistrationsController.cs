using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KoiMuseum.Data.Models;

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
            var fa24Se172594Prn231G1KfsContext = _context.Registrations.Include(r => r.Contest).Include(r => r.RegisterDetail);
            return View(await fa24Se172594Prn231G1KfsContext.ToListAsync());
        }

        // GET: Registrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .Include(r => r.Contest)
                .Include(r => r.RegisterDetail)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // GET: Registrations/Create
        public IActionResult Create()
        {
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name");
            ViewData["RegisterDetailId"] = new SelectList(_context.RegisterDetails, "Id", "Id");
            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ContestId,RegisterDetailId,RegistrationDate,ApprovalDate,RejectedReason,ConfirmationCode,IntroductionOfOwner,IntroductionOfKoi,AdminReviewedBy,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            ViewData["ContestId"] = new SelectList(_context.Contests, "Id", "Name", registration.ContestId);
            ViewData["RegisterDetailId"] = new SelectList(_context.RegisterDetails, "Id", "Id", registration.RegisterDetailId);
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                try
                {
                    _context.Update(registration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.Id))
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

            var registration = await _context.Registrations
                .Include(r => r.Contest)
                .Include(r => r.RegisterDetail)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration != null)
            {
                _context.Registrations.Remove(registration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationExists(int id)
        {
            return _context.Registrations.Any(e => e.Id == id);
        }
    }
}
