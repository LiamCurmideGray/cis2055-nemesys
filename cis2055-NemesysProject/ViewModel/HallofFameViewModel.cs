using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis2055_NemesysProject.Models;

namespace cis2055_NemesysProject.ViewModel
{
    public class HallofFameViewModel
    {
        public IEnumerable<HallofFameListViewModel> TotalReportsOfReporter { get; set; }
        public IEnumerable<Report> Report { get; set; }

      


    }
}
