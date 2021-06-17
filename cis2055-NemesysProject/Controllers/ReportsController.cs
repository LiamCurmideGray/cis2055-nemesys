using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cis2055_NemesysProject.Data;
using cis2055_NemesysProject.Models;
using cis2055_NemesysProject.ViewModel;
using cis2055_NemesysProject.Data.Interfaces;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace cis2055_NemesysProject.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly ILogger<ReportsController> _logger;

        private readonly UserManager<NemesysUser> _userManager;

        public ReportsController(UserManager<NemesysUser> userManager, IReportRepository reportRepository, ILogger<ReportsController> logger)
        {
            _userManager = userManager;
            _reportRepository = reportRepository;
            _logger = logger;
        }

        // GET: Reports
        [Authorize]
        public IActionResult Index()
        {

            var model = new ReportListViewModel()
            {
                TotalReports = _reportRepository.GetAllReports().Count(),
                Reports = _reportRepository.GetAllReports().OrderByDescending(d => d.DateOfReport),
                StatusCategories = _reportRepository.GetAllStatusCategories(),
                OwnReports = false
            };
            return View(model);
        }

        [Authorize]
        public IActionResult MyReport(string id)
        {
            var model = new ReportListViewModel()
            {
                TotalReports = _reportRepository.GetAllReports().Count(),
                Reports = _reportRepository.GetReportByUserId(id).OrderByDescending(d => d.DateOfReport),
                StatusCategories = _reportRepository.GetAllStatusCategories(),
                OwnReports = true
            };
            return View("Index", model);
        }

        // GET: Reports/Details/5
        public IActionResult Details(int id)
        {
            var model = _reportRepository.GetReportById(id);
            return View(model);
        }

        // GET: Reports/Create
        [Authorize(Roles = "Reporter")]
        public IActionResult Create()
        {
            var model = new CreateReportViewModel()
            {
                HazardList = _reportRepository.GetAllHazard()
            };
            return View(model);
        }

        // POST: Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Reporter")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("HazardId,DateTimeHazard,Title,Description,ImageToUpload,Latitude,Longitude")] CreateReportViewModel report)
        {
            if (ModelState.IsValid)
            {
                report.UserId = _userManager.GetUserId(User);
                _reportRepository.AddReport(report);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var model = new CreateReportViewModel()
                {
                    HazardList = _reportRepository.GetAllHazard()
                };
                return View(model);
            }
        }

        // GET: Reports/Edit/5
        [Authorize(Roles = "Reporter")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var report = _reportRepository.GetReportById(id);
            var currentUser = await _userManager.GetUserAsync(User);
            if (report.UserId == currentUser.Id)
            {
                CreateReportViewModel model = new CreateReportViewModel()
                {
                    ReportId = report.ReportId,
                    UserId = report.User.Id,
                    HazardId = report.HazardId,
                    DateTimeHazard = report.DateTimeHazard,
                    Title = report.Title,
                    Description = report.Description,
                    Image = report.Image,
                    Longitude = report.Longitude,
                    Latitude = report.Latitude,
                    HazardList = _reportRepository.GetAllHazard()
                };
                return View(model);
            }
            else
            {
                return Unauthorized();
            }


        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Reporter")]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,HazardId,DateTimeHazard,Title,Description,ImageToUpload,Latitude,Longitude")] CreateReportViewModel report)
        {

            if (ModelState.IsValid)
            {
                report.ReportId = id;
                var currentUser = await _userManager.GetUserAsync(User);
                bool result = _reportRepository.UpdateReport(report, currentUser.Id);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(report);
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        // GET: Reports/Delete/5
        [Authorize(Roles = "Reporter")]
        public async Task<IActionResult> Delete(int id)
        {
            var report = _reportRepository.GetReportById(id);
            if (report == null)
            {
             return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (report.User.Id == currentUser.Id)
            {
                return View(report);

            }
            else
            {
                return Unauthorized();
            }

        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Reporter")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            bool result = _reportRepository.DeleteReport(id, currentUser.Id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Unauthorized();
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Reporter")]
        public IActionResult UpdateUpvote(int id)
        {
            Report report = _reportRepository.GetReportById(id);
            string userId = _userManager.GetUserId(User);

            if (!report.UserId.Equals(userId))
            {
                _reportRepository.UpdateReportUpVote(id);
            }

            return RedirectToAction(nameof(Index));
        }

      
    }
}
