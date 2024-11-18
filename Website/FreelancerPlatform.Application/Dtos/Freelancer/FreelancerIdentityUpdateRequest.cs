using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Freelancer
{
    public class FreelancerIdentityUpdateRequest
    {
		public string Identity { get; set; }
		public string BankNumber { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
	}
}
