using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cis2055_NemesysProject.Data;
using cis2055_NemesysProject.Models;
using cis2055_NemesysProject.ViewModel;
using cis2055_NemesysProject.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace cis2055_NemesysProject.Controllers
{
    public class HallofFameController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly UserManager<NemesysUser> _usermanager;

        public HallofFameController( IReportRepository reportRepository, UserManager<NemesysUser> userManager)
        {
            _reportRepository = reportRepository;
            _usermanager = userManager;
        }

        // GET: HallofFame
        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<Report> reports = _reportRepository.GetAllReports();
            List<HallofFameListViewModel> reportsPerUser = new List<HallofFameListViewModel>();
            List<string> UserId = new List<string>();

            foreach(var item in reports)
            {
                if(!UserId.Contains(item.UserId))
                {
                    HallofFameListViewModel hallofFameListViewModel = new HallofFameListViewModel();
                    hallofFameListViewModel.UserIds = item.UserId;
                    hallofFameListViewModel.AuthorAlias = _usermanager.FindByIdAsync(item.UserId).Result.AuthorAlias;
                    hallofFameListViewModel.TotalReportsCount = _reportRepository.GetReportByUserId(item.UserId).Count();
                    hallofFameListViewModel.TotalUpvotesCount = 0;
                    hallofFameListViewModel.Top3Reports = _reportRepository.GetReportByUserId(item.UserId).OrderByDescending(u => u.Upvotes).Take(3);

                    foreach(var p in reports.Where(r => r.UserId.Equals(hallofFameListViewModel.UserIds)))
                    {
                        hallofFameListViewModel.TotalUpvotesCount += p.Upvotes;
                    }

                    reportsPerUser.Add(hallofFameListViewModel);

                    UserId.Add(item.UserId);
                } 
            }

            var model = new HallofFameViewModel()
            {
                TotalReportsOfReporter = reportsPerUser,
                Report = reports
            };

            return View(model);
        }
    }
}
