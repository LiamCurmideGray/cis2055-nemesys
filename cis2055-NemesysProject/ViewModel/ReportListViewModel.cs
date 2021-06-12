using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cis2055_NemesysProject.ViewModel
{
    public class ReportListViewModel
    {
        public int TotalReports { get; set; }
        public IEnumerable<ReportViewModel> Reports { get; set; }
    }
}
