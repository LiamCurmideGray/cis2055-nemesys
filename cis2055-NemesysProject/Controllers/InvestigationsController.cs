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

        [Authorize(Roles = "Investigator")]
        // GET: Investigations
        public async Task<IActionResult> Index()
        {
            //var cis2055nemesysContext = _context.Investigations.Include(i => i.Report).Include(i => i.User);
            var model = new InvestigationListViewModel()
            {
                TotalInvestigations = _nemesysRepository.GetAllInvestigations().Count(),
                Investigations = _nemesysRepository.GetAllInvestigations().OrderByDescending(i => i.InvestigationId).Select(i => new InvestigationViewModel
                {
                    InvestigationId = i.InvestigationId,
                    ReportId = i.ReportId,
                    Report = new Report()
                    {
                        ReportId = i.Report.ReportId,
                        Description = i.Report.Description,
                        StatusId = i.Report.StatusId,
                        UserId = i.Report.UserId,
                        User = _userManager.FindByIdAsync(i.Report.UserId).Result
                    },
                    Description = i.Description,
                    User = new NemesysUser()
                    {
                        Id = i.User.Id,
                        UserName = i.User.UserName,
                        AuthorAlias = i.User.AuthorAlias
                    }
                })
            };
            return View(model);
        }

        [Authorize(Roles = "Investigator")]
        // GET: Investigations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var reportId = _context.Investigations.Where(i => i.InvestigationId == id).Select(i => i.ReportId).FirstOrDefault();
            var reportInvestigation = _nemesysRepository.GetReportById(reportId);
            var investigation = await _context.Investigations
                .Include(i => i.Report)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.InvestigationId == id);

            var model = new InvestigationViewModel()
            {
                InvestigationId = investigation.InvestigationId,
                ReportId = investigation.ReportId,
                Report = new Report()
                {
                    ReportId = investigation.Report.ReportId,
                    Description = investigation.Report.Description,
                    DateOfReport = investigation.Report.DateOfReport,
                    DateTimeHazard = reportInvestigation.DateTimeHazard,
                    Status = new StatusCategory()
                    {
                        StatusId = reportInvestigation.Status.StatusId,
                        StatusType = reportInvestigation.Status.StatusType
                    },
                    Hazard = new Hazard()
                    {
                        HazardId = reportInvestigation.Hazard.HazardId,
                        HazardType = reportInvestigation.Hazard.HazardType
                    },
                    User = new NemesysUser()
                    {
                        Id = reportInvestigation.User.Id,
                        UserName = reportInvestigation.User.UserName,
                        AuthorAlias = reportInvestigation.User.AuthorAlias,
                        PhoneNumber = reportInvestigation.User.PhoneNumber,
                        Email = reportInvestigation.User.Email
                    },
                    Image = reportInvestigation.Image
                },
                Description = investigation.Description,
                User = new NemesysUser()
                {
                    Id = investigation.User.Id,
                    UserName = investigation.User.UserName,
                    AuthorAlias = investigation.User.AuthorAlias,
                    PhoneNumber = investigation.User.PhoneNumber,
                    Email = investigation.User.Email
                },
            };
            if (investigation == null)
            {
                return NotFound();
            }

            return View(model);
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
                    StatusList = statusList,
                    User = _userManager.GetUserAsync(User).Result
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
        public IActionResult Create(int id, [Bind("StatusId,Description")] CreateInvestigationViewModel investigation)
        {
            if (ModelState.IsValid)
            {
                var reportInvestigation = _context.Investigations.FirstOrDefault(i => i.ReportId == id);
                var report = _nemesysRepository.GetReportById(id);
                var currUser = _userManager.GetUserAsync(User);
                if (reportInvestigation == null)
                {
                    var newInvestigation = new Investigation()
                    {
                        ReportId = id,
                        Description = investigation.Description,
                        UserId = _userManager.GetUserId(User),
                        Report = report,
                        
                    };
                    report.StatusId = investigation.StatusId;
                    _context.Add(newInvestigation);
                    _context.SaveChanges();
                    newInvestigation.User = _userManager.GetUserAsync(User).Result;
                    _context.Update(newInvestigation);
                    _context.SaveChanges();

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

        [Authorize(Roles = "Investigator")]
        // GET: Investigations/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var investigation = _nemesysRepository.GetInvestigationById(id);
            var currUser = _userManager.GetUserId(User);

            if (currUser == investigation.UserId)
            {
                var statusList = _context.StatusCategories.ToList();

                var model = new CreateInvestigationViewModel()
                {
                    InvestigationId = id,
                    StatusId = investigation.Report.StatusId,
                    StatusList = statusList,
                    Description = investigation.Description,
                };
                return View(model);
            }
            else
            {
                return Unauthorized();
            }
        }

        // POST: Investigations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Investigator")]
        public async Task<IActionResult> Edit(int id, [Bind("StatusId,Description")] CreateInvestigationViewModel investigation)
        {
            //if (id != investigation.InvestigationId)
            //{
            //    return NotFound();
            //}
            var inv = _nemesysRepository.GetInvestigationById(id);
            var reportId = inv.ReportId;
            var report = _nemesysRepository.GetReportById(reportId);
            var currUser = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                try
                {
                    if (currUser == inv.UserId)
                    {
                        inv.Description = investigation.Description;
                        report.StatusId = investigation.StatusId;
                        //report.StatusId = investigation.StatusId;
                        _context.Update(inv);
                        _context.Update(report);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return Unauthorized();
                    }
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
            else
            {
                var statusList = _context.StatusCategories.ToList();

                var model = new CreateInvestigationViewModel()
                {
                    InvestigationId = id,
                    StatusId = inv.Report.StatusId,
                    StatusList = statusList,
                    Description = inv.Description,
                };
                return View(model);
            }
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
