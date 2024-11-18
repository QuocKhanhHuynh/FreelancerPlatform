using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Freelancer
{
    public class FreelancerPaymentUpdateRequest
    {
        public string BankName { get; set; }
        public string BankNumber { get; set; }
    }
}
