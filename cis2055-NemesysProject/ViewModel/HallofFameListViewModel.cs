using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cis2055_NemesysProject.Models;

namespace cis2055_NemesysProject.ViewModel
{
    public class HallofFameListViewModel
    {
        public string UserIds { get; set; }
        public string AuthorAlias { get; set; }
        public int TotalReportsCount { get; set; }
        public int TotalUpvotesCount { get; set; }
        public IEnumerable<Report> Top3Reports { get; set; }
    }
}
