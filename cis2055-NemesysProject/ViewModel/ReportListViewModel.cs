using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis2055_NemesysProject.Models;

namespace cis2055_NemesysProject.ViewModel
{
    public class ReportListViewModel
    {
        public int TotalReports { get; set; }
        public IEnumerable<Report> Reports { get; set; }
        public IEnumerable<StatusCategory> StatusCategories { get; set; }
        public bool OwnReports { get; set; }
    }
}
