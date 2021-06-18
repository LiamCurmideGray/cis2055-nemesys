using cis2055_NemesysProject.Data.Interfaces;
using cis2055_NemesysProject.Models;
using cis2055_NemesysProject.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace cis2055_NemesysProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IReportRepository _reportRepository;

        public HomeController(ILogger<HomeController> logger, IReportRepository reportRepository)
        {
            _logger = logger;
            _reportRepository = reportRepository;
        }

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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
