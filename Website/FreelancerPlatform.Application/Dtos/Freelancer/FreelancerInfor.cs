using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Freelancer
{
    public class FreelancerInfor
    {
        public string? FullName { get; set; }
        public DateTime? BirthDay { get; set; }
        public int? Gender { get; set; }
        public string? ImageUrl { get; set; }
        public string? Experence { get; set; }
        public string? Skill { get; set; }
		public int? Address { get; set; }
	}
}
