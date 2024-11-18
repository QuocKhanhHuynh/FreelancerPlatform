using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Apply
{
	public class ApplyOfJobQuickViewModel
	{
		public int Id { get; set; }
		public int Deal {  get; set; }
		public int ExecutionDay { get; set; }
		public string Introduction { get; set; }
		public DateTime CreateDay { get; set; }
		public string LastName { get; set; }
        public string FirstName { get; set; }
        public string ImageUrl { get; set; }
		public int FreelancerId { get; set; }
		public bool IsOffer { get; set; }
	}
}
