﻿using System;
using System.Collections.Generic;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class Report
    {
        public Report()
        {
            Investigations = new HashSet<Investigation>();
            ReportHazards = new HashSet<ReportHazard>();
        }

        public int ReportId { get; set; }
        public int UserId { get; set; }
        public int PinpointId { get; set; }
        public DateTime DateOfReport { get; set; }
        public DateTime DateTimeHazard { get; set; }
        public string Description { get; set; }
        public int Upvotes { get; set; }
        public byte[] Image { get; set; }

        public virtual Pinpoint Pinpoint { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Investigation> Investigations { get; set; }
        public virtual ICollection<ReportHazard> ReportHazards { get; set; }
    }
}
