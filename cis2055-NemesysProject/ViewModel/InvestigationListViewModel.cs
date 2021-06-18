using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis2055_NemesysProject.Models;

namespace cis2055_NemesysProject.ViewModel
{
    public class InvestigationListViewModel
    {
        public int TotalInvestigations { get; set; }
        public IEnumerable<Investigation> Investigations { get; set; }
    }
}
