using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace cis2055_NemesysProject.Models
{
    public class NemesysUser  : IdentityUser
    {
        [PersonalData]
        public string AuthorAlias { get; set; }
    }
}
