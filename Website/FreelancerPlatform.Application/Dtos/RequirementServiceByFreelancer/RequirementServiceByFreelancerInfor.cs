using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.RequirementServiceByFreelancer
{
    public class RequirementServiceByFreelancerInfor
    {
        public int Status { get; set; }
        public int ServiceForFreelancerId { get; set; }
        public int FreelancerId { get; set; }
    }
}
