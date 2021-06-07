using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cis2055_NemesysProject.ViewModel
{
    public class CreateReportViewModel
    {
        public int UserId { get; set; }

        public DateTime DateTimeHazard { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage = "Report description cannot be empty.")]
        public string Description { get; set; }

        [Display(Name = "Hazard Image")]
        public IFormFile ImageToUpload { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
