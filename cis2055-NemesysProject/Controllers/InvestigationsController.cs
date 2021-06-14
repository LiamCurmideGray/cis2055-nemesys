using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cis2055_NemesysProject.Data;
using cis2055_NemesysProject.Models;
using Microsoft.AspNetCore.Authorization;
using cis2055_NemesysProject.ViewModel;
using Microsoft.AspNetCore.Identity;
using cis2055_NemesysProject.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace cis2055_NemesysProject.Controllers
{
    public class InvestigationsController : Controller
    {
        private readonly cis2055nemesysContext _context;
        private readonly UserManager<NemesysUser> _userManager;
        private readonly INemesysRepository _nemesysRepository;
        private readonly ILogger<ReportsController> _logger;

        public InvestigationsController(cis2055nemesysContext context, UserManager<NemesysUser> userManager, INemesysRepository nemesysRepository, ILogger<ReportsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _nemesysRepository = nemesysRepository;
        }

        // GET: Investigations
        public async Task<IActionResult> Index()
        {
            var cis2055nemesysContext = _context.Investigations.Include(i => i.Report).Include(i => i.User);
            return View(await cis2055nemesysContext.ToListAsync());
        }

        // GET: Investigations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investigation = await _context.Investigations
                .Include(i => i.Report)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.InvestigationId == id);
            if (investigation == null)
            {
                return NotFound();
            }

            return View(investigation);
        }

        [Authorize(Roles = "Investigator")]
        // GET: Investigations/Create
        public IActionResult Create(int id)
        {
            //ViewData["ReportId"] = new SelectList(_context.Reports, "ReportId", "Description");
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            //ViewData["StatusId"] = new SelectList(_context.StatusCategories, "StatusId", "StatusType");
            var reportInvestigation = _context.Investigations.FirstOrDefault(i => i.ReportId == id);
            if (reportInvestigation == null)
            {
                var statusList = _context.StatusCategories.ToList();

                var model = new CreateInvestigationViewModel()
                {
                    ReportId = id,
                    StatusId = 1,
                    StatusList = statusList
                };
                return View(model);
            }
            else
            {
                ViewData["ReportId"] = id;
                return View("InvestigationError");
            }
        }

        // POST: Investigations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Investigator")]
        public async Task<IActionResult> Create(int id, [Bind("StatusId,Description")] CreateInvestigationViewModel investigation)
        {
            if (ModelState.IsValid)
            {
                var reportInvestigation = _context.Investigations.FirstOrDefault(i => i.ReportId == id);
                var report = _nemesysRepository.GetReportById(id);
                if (reportInvestigation == null)
                {
                    var newInvestigation = new Investigation()
                    {
                        ReportId = id,
                        Description = investigation.Description,
                        UserId = _userManager.GetUserId(User)
                    };
                    report.StatusId = investigation.StatusId;
                    _context.Add(newInvestigation);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["ReportId"] = id;
                    return View("InvestigationError");
                }
            }
            else
            {
                var statusList = _context.StatusCategories.ToList();

                var model = new CreateInvestigationViewModel()
                {
                    StatusList = statusList,
                    StatusId = 1
                };
                return View(model);
            }
        }

        // GET: Investigations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investigation = await _context.Investigations.FindAsync(id);
            if (investigation == null)
            {
                return NotFound();
            }
            ViewData["ReportId"] = new SelectList(_context.Reports, "ReportId", "Description", investigation.ReportId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", investigation.UserId);
            return View(investigation);
        }

        // POST: Investigations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvestigationId,UserId,ReportId")] Investigation investigation)
        {
            if (id != investigation.InvestigationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(investigation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvestigationExists(investigation.InvestigationId))
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
            ViewData["ReportId"] = new SelectList(_context.Reports, "ReportId", "Description", investigation.ReportId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", investigation.UserId);
            return View(investigation);
        }

        // GET: Investigations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investigation = await _context.Investigations
                .Include(i => i.Report)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.InvestigationId == id);
            if (investigation == null)
            {
                return NotFound();
            }

            return View(investigation);
        }

        // POST: Investigations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var investigation = await _context.Investigations.FindAsync(id);
            _context.Investigations.Remove(investigation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvestigationExists(int id)
        {
            return _context.Investigations.Any(e => e.InvestigationId == id);
        }
    }
}
