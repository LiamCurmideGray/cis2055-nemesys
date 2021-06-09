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
    public class HazardsController : Controller
    {
        private readonly cis2055nemesysContext _context;

        public HazardsController(cis2055nemesysContext context)
        {
            _context = context;
        }

        // GET: Hazards
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hazards.ToListAsync());
        }

        // GET: Hazards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hazard = await _context.Hazards
                .FirstOrDefaultAsync(m => m.HazardId == id);
            if (hazard == null)
            {
                return NotFound();
            }

            return View(hazard);
        }

        // GET: Hazards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hazards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HarzardId,HazardType")] Hazard hazard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hazard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hazard);
        }

        // GET: Hazards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hazard = await _context.Hazards.FindAsync(id);
            if (hazard == null)
            {
                return NotFound();
            }
            return View(hazard);
        }

        // POST: Hazards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HarzardId,HazardType")] Hazard hazard)
        {
            if (id != hazard.HazardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hazard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HazardExists(hazard.HazardId))
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
            return View(hazard);
        }

        // GET: Hazards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hazard = await _context.Hazards
                .FirstOrDefaultAsync(m => m.HazardId == id);
            if (hazard == null)
            {
                return NotFound();
            }

            return View(hazard);
        }

        // POST: Hazards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hazard = await _context.Hazards.FindAsync(id);
            _context.Hazards.Remove(hazard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HazardExists(int id)
        {
            return _context.Hazards.Any(e => e.HazardId == id);
        }
    }
}
