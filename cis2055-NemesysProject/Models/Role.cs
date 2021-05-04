using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

#nullable disable

namespace cis2055_NemesysProject.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string RoleType { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual ICollection<User> Users { get; set; }
    }
}
