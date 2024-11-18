using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Account
{
    public class VerificationRequest
    {
        public int AccountId { get; set; }
        public bool Verification { get; set; }
        public int SystemManagementId { get; set; }
    }
}
