using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Domain.Entity
{
	public class FreelancerSkill
	{
		public int FreelancerId	{ get; set; }
		public Freelancer Freelancer { get; set; }

		public int SkillId { get; set; }
		public SKill SKill { get; set; }
	}
}
