using System;
using System.Collections.Generic;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class LogInvestigation
    {
        public int LogInvestigationId { get; set; }
        public int InvestigationId { get; set; }
        public string Description { get; set; }
        public DateTime DateOfAction { get; set; }

        public virtual Investigation Investigation { get; set; }
    }
}
