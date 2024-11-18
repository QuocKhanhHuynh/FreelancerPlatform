using FreelancerPlatform.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Domain.Entity
{
    public class SystemManagementRole : EntityBase
    {
        public int SystemManagementId { get; set; }
        public SystemManagement SystemManagement { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
