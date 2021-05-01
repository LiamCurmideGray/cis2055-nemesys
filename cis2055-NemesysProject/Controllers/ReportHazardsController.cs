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
    public class ReportHazardsController : Controller
    {
        private readonly cis2055nemesysContext _context;

        public ReportHazardsController(cis2055nemesysContext context)
        {
            _context = context;
        }

        // GET: ReportHazards
        public async Task<IActionResult> Index()
        {
            var cis2055nemesysContext = _context.ReportHazards.Include(r => r.Hazard).Include(r => r.Report);
            return View(await cis2055nemesysContext.ToListAsync());
        }

        // GET: ReportHazards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportHazard = await _context.ReportHazards
                .Include(r => r.Hazard)
                .Include(r => r.Report)
                .FirstOrDefaultAsync(m => m.HazardId == id);
            if (reportHazard == null)
            {
                return NotFound();
            }

            return View(reportHazard);
        }

        // GET: ReportHazards/Create
        public IActionResult Create()
        {
            ViewData["HazardId"] = new SelectList(_context.Hazards, "HarzardId", "HazardType");
            ViewData["ReportId"] = new SelectList(_context.Reports, "ReportId", "Description");
            return View();
        }

        // POST: ReportHazards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HazardId,ReportId")] ReportHazard reportHazard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reportHazard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HazardId"] = new SelectList(_context.Hazards, "HarzardId", "HazardType", reportHazard.HazardId);
            ViewData["ReportId"] = new SelectList(_context.Reports, "ReportId", "Description", reportHazard.ReportId);
            return View(reportHazard);
        }

        // GET: ReportHazards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportHazard = await _context.ReportHazards.FindAsync(id);
            if (reportHazard == null)
            {
                return NotFound();
            }
            ViewData["HazardId"] = new SelectList(_context.Hazards, "HarzardId", "HazardType", reportHazard.HazardId);
            ViewData["ReportId"] = new SelectList(_context.Reports, "ReportId", "Description", reportHazard.ReportId);
            return View(reportHazard);
        }

        // POST: ReportHazards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HazardId,ReportId")] ReportHazard reportHazard)
        {
            if (id != reportHazard.HazardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reportHazard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportHazardExists(reportHazard.HazardId))
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
            ViewData["HazardId"] = new SelectList(_context.Hazards, "HarzardId", "HazardType", reportHazard.HazardId);
            ViewData["ReportId"] = new SelectList(_context.Reports, "ReportId", "Description", reportHazard.ReportId);
            return View(reportHazard);
        }

        // GET: ReportHazards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportHazard = await _context.ReportHazards
                .Include(r => r.Hazard)
                .Include(r => r.Report)
                .FirstOrDefaultAsync(m => m.HazardId == id);
            if (reportHazard == null)
            {
                return NotFound();
            }

            return View(reportHazard);
        }

        // POST: ReportHazards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reportHazard = await _context.ReportHazards.FindAsync(id);
            _context.ReportHazards.Remove(reportHazard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportHazardExists(int id)
        {
            return _context.ReportHazards.Any(e => e.HazardId == id);
        }
    }
}
