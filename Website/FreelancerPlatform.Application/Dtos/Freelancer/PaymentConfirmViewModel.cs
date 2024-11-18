using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Freelancer
{
    public class PaymentConfirmViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime UpdateDay { get; set; }
        public bool VerifyStatus { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
    }
}
