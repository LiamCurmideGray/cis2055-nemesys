using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cis2055_NemesysProject.Data;
using cis2055_NemesysProject.Models;

namespace cis2055_NemesysProject.Controllers
{
    public class LogInvestigationsController : Controller
    {
        private readonly cis2055nemesysContext _context;

        public LogInvestigationsController(cis2055nemesysContext context)
        {
            _context = context;
        }

        // GET: LogInvestigations
        public async Task<IActionResult> Index()
        {
            var cis2055nemesysContext = _context.LogInvestigations.Include(l => l.Investigation);
            return View(await cis2055nemesysContext.ToListAsync());
        }

        // GET: LogInvestigations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logInvestigation = await _context.LogInvestigations
                .Include(l => l.Investigation)
                .FirstOrDefaultAsync(m => m.LogInvestigationId == id);
            if (logInvestigation == null)
            {
                return NotFound();
            }

            return View(logInvestigation);
        }

        // GET: LogInvestigations/Create
        public IActionResult Create()
        {
            ViewData["InvestigationId"] = new SelectList(_context.Investigations, "InvestigationId", "InvestigationId");
            return View();
        }

        // POST: LogInvestigations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LogInvestigationId,InvestigationId,Description,DateOfAction")] LogInvestigation logInvestigation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(logInvestigation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvestigationId"] = new SelectList(_context.Investigations, "InvestigationId", "InvestigationId", logInvestigation.InvestigationId);
            return View(logInvestigation);
        }

        // GET: LogInvestigations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logInvestigation = await _context.LogInvestigations.FindAsync(id);
            if (logInvestigation == null)
            {
                return NotFound();
            }
            ViewData["InvestigationId"] = new SelectList(_context.Investigations, "InvestigationId", "InvestigationId", logInvestigation.InvestigationId);
            return View(logInvestigation);
        }

        // POST: LogInvestigations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LogInvestigationId,InvestigationId,Description,DateOfAction")] LogInvestigation logInvestigation)
        {
            if (id != logInvestigation.LogInvestigationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(logInvestigation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogInvestigationExists(logInvestigation.LogInvestigationId))
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
            ViewData["InvestigationId"] = new SelectList(_context.Investigations, "InvestigationId", "InvestigationId", logInvestigation.InvestigationId);
            return View(logInvestigation);
        }

        // GET: LogInvestigations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logInvestigation = await _context.LogInvestigations
                .Include(l => l.Investigation)
                .FirstOrDefaultAsync(m => m.LogInvestigationId == id);
            if (logInvestigation == null)
            {
                return NotFound();
            }

            return View(logInvestigation);
        }

        // POST: LogInvestigations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logInvestigation = await _context.LogInvestigations.FindAsync(id);
            _context.LogInvestigations.Remove(logInvestigation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogInvestigationExists(int id)
        {
            return _context.LogInvestigations.Any(e => e.LogInvestigationId == id);
        }
    }
}
