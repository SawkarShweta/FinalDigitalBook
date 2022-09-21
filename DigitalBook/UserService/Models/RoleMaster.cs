using System;
using System.Collections.Generic;

namespace UserService.Models
{
    public partial class RoleMaster
    {
        public RoleMaster()
        {
            UserMasters = new HashSet<UserMaster>();
        }

        public int RoleId { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<UserMaster> UserMasters { get; set; }
    }
}
