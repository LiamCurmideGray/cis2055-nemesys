using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace cis2055_NemesysProject.Models
{
    public class NemesysUser  : IdentityUser
    {
        [PersonalData]
        public string AuthorAlias { get; set; }
        [Display (Name = "Phone Number")]
        public override string PhoneNumber { get; set; }
    }
}
