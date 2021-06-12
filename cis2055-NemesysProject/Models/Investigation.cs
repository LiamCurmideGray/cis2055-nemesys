using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class Investigation
    {
        public Investigation()
        {
            LogInvestigations = new HashSet<LogInvestigation>();
        }

        [Key]
        public int InvestigationId { get; set; }
        public int ReportId { get; set; }
        public string Description { get; set; }
        public virtual Report Report { get; set; }
        public string UserId { get; set; }
        public virtual NemesysUser User { get; set; }
        public virtual ICollection<LogInvestigation> LogInvestigations { get; set; }
    }
}
