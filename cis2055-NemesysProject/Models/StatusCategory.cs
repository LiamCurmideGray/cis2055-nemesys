using System;
using System.Collections.Generic;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class StatusCategory
    {
        public StatusCategory()
        {
            Reports = new HashSet<Report>();
        }

        public int StatusId { get; set; }
        public string StatusType { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}
