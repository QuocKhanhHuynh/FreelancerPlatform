using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.SystemManagement
{
    public class SystemManagementRegisterRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
