using System;
using System.Collections.Generic;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class ReportHazard
    {
        public int HazardId { get; set; }
        public int ReportId { get; set; }

        public virtual Hazard Hazard { get; set; }
        public virtual Report Report { get; set; }
    }
}
