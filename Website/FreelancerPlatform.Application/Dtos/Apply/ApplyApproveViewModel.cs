using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Apply
{
    public class ApplyApproveViewModel
    {
        public string ImageUrl { get; set; }
        public string FreelancerName { get; set; }
        public int JobId { get; set; }
        public DateTime ApproveDay { get; set; }
        public int FreelancerId { get; set; }
    }
}
