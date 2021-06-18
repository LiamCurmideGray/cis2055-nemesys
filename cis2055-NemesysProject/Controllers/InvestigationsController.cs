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
        private readonly InterfaceInvestigationRepository _investigationRepository;
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<ReportsController> _logger;

        public InvestigationsController(cis2055nemesysContext context, UserManager<NemesysUser> userManager, IReportRepository reportRepository ,
            InterfaceInvestigationRepository investigationRepository, ILogger<ReportsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _investigationRepository = investigationRepository;
            _reportRepository = reportRepository;
        }

        [Authorize]
        // GET: Investigations
        public IActionResult Index()
        {
            //var cis2055nemesysContext = _context.Investigations.Include(i => i.Report).Include(i => i.User);
            var model = new InvestigationListViewModel()
            {
                TotalInvestigations = _investigationRepository.GetAllInvestigations().Count(),
                Investigations = _investigationRepository.GetAllInvestigations().OrderByDescending(i => i.InvestigationId)
            };
            return View(model);
        }

        [Authorize]
        // GET: Investigations/Details/5
        public IActionResult Details(int id)
        {
            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }

            var investigation = _investigationRepository.GetInvestigationById(id);
           
            if (investigation == null)
            {
                return RedirectToAction(nameof(Index));
                //nneds report user, report hazard, report status
            }

            return View(investigation);
        }

        [Authorize(Roles = "Investigator")]
        // GET: Investigations/Create
        public IActionResult Create(int id)
        {
            
            var reportInvestigation = _investigationRepository.GetInvestigationByReportId(id);
            if (reportInvestigation == null)
            {
                var statusList = _reportRepository.GetAllStatusCategories();

                var model = new CreateInvestigationViewModel()
                {
                    ReportId = id,
                    StatusId = 3,
                    StatusList = statusList,
                    User = _userManager.GetUserAsync(User).Result,
                    ReportTitle = _reportRepository.GetReportById(id).Title
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
        public IActionResult Create(int id, [Bind("StatusId,Description,LogDescription")] CreateInvestigationViewModel investigation)
        {
            if (ModelState.IsValid)
            {
                var reportInvestigation = _context.Investigations.FirstOrDefault(i => i.ReportId == id);
                var report = _reportRepository.GetReportById(id);
                //var currUser = _userManager.GetUserAsync(User);
                if (reportInvestigation == null)
                {
                    var newInvestigation = new Investigation()
                    {
                        ReportId = id,
                        Description = investigation.Description,
                        UserId = _userManager.GetUserId(User),
                        //Report = report,

                    };

                    report.StatusId = investigation.StatusId;
                    _context.Add(newInvestigation);
                    _context.SaveChanges();

                    Investigation investigationId = _context.Investigations.FirstOrDefault(i => i.ReportId == id);
                    LogInvestigation logInvestigation = new LogInvestigation()
                    {
                        InvestigationId = investigationId.InvestigationId,
                        Description = investigation.LogDescription,
                        DateOfAction = DateTime.Now
                    };

                    _context.Add(logInvestigation);
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

            var investigation = _investigationRepository.GetInvestigationById(id);
            var currUser = _userManager.GetUserId(User);
            var loginvestigation = _investigationRepository.GetLogsOfInvestigation(investigation.InvestigationId);

            if (currUser == investigation.UserId)
            {
                var statusList = _context.StatusCategories.ToList();

                var model = new CreateInvestigationViewModel()
                {
                    InvestigationId = id,
                    StatusId = investigation.Report.StatusId,
                    StatusList = statusList,
                    Description = investigation.Description,
                    LogInvestigation = loginvestigation
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
        public async Task<IActionResult> Edit(int id, [Bind("StatusId,Description,LogDescription")] CreateInvestigationViewModel investigation)
        {
            //if (id != investigation.InvestigationId)
            //{
            //    return NotFound();
            //}
            var inv = _investigationRepository.GetInvestigationById(id);
            var reportId = inv.ReportId;
            var report = _reportRepository.GetReportById(reportId);
            var currUser = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                try
                {
                    if (currUser == inv.UserId)
                    {
                        inv.Description = investigation.Description;
                        report.StatusId = investigation.StatusId;
                        _context.Update(inv);
                        _context.Update(report);

                        //string datenow = DateTime.Now.ToString();
                        ////var dateTime = DateTime.Parse(datenow);
                        //int minute = DateTime.Now.Minute;
                        //int hour = DateTime.Now.Hour;

                        LogInvestigation log = new LogInvestigation()
                        {
                            InvestigationId = inv.InvestigationId,
                            Description = investigation.LogDescription,
                            DateOfAction = DateTime.Now

                        };
                        _context.Add(log);
                        _context.SaveChanges();
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
                var refreshInv = _investigationRepository.GetInvestigationById(id);
                var statusList = _context.StatusCategories.ToList();
                var loginvestigation = _investigationRepository.GetLogsOfInvestigation(refreshInv.InvestigationId);

                var model = new CreateInvestigationViewModel()
                {
                    InvestigationId = id,
                    StatusId = inv.Report.StatusId,
                    StatusList = statusList,
                    Description = inv.Description,
                    LogInvestigation = loginvestigation
                };
                return View(model);
            }
        }

        // GET: Investigations/Delete/5
        [Authorize(Roles = "Investigator")]
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

            var currentuser = _userManager.GetUserAsync(User);
            if (currentuser.Result.Id.Equals(investigation.UserId))
            {
                if (investigation == null)
                {
                    return NotFound();
                }

                return View(investigation);

            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Investigations/Delete/5
        [Authorize(Roles = "Investigator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var investigation = await _context.Investigations.FindAsync(id);
            var currentuser = _userManager.GetUserAsync(User);
            if (currentuser.Result.Id.Equals(investigation.UserId))
            {
                var logs = _investigationRepository.GetLogsOfInvestigation(investigation.InvestigationId);

                foreach(var item in logs)
                {
                _context.LogInvestigations.Remove(item);
                }
                _context.Investigations.Remove(investigation);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool InvestigationExists(int id)
        {
            return _context.Investigations.Any(e => e.InvestigationId == id);
        }
    }
}
