using System;
using System.Collections.Generic;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class Investigation
    {
        public Investigation()
        {
            LogInvestigations = new HashSet<LogInvestigation>();
        }

        public int InvestigationId { get; set; }
        public int UserId { get; set; }
        public int ReportId { get; set; }
        public string Description { get; set; }
        public virtual Report Report { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<LogInvestigation> LogInvestigations { get; set; }
    }
}
