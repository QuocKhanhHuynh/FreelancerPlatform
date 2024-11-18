using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.SystemManagement
{
    public class SystemManagementViewModel : SystemManagementInfor
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool Status { get; set; }
        public List<int> RoleId { get; set; }
    }
}
