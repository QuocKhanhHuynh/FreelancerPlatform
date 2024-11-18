using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Apply
{
    public class ApplyCreateRequest : ApplyInfor
    {
        public int FreelancerId { get; set; }
        public int JobId { get; set; }
        public bool IsOffer { get; set; } = false;
    }
}
