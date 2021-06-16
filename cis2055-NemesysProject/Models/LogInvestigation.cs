using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class LogInvestigation
    {
        [Key]
        public int LogInvestigationId { get; set; }
        public int InvestigationId { get; set; }
        [Display(Name = "Log Description")]
        public string Description { get; set; }
        [Display(Name = "Date of Action")]
        [DataType(DataType.DateTime)]
        public DateTime DateOfAction { get; set; }

        public virtual Investigation Investigation { get; set; }
    }
}
