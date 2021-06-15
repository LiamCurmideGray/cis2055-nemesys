using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cis2055_NemesysProject.ViewModel
{
    public class InvestigationListViewModel
    {
        public int TotalInvestigations { get; set; }
        public IEnumerable<InvestigationViewModel> Investigations { get; set; }
    }
}
