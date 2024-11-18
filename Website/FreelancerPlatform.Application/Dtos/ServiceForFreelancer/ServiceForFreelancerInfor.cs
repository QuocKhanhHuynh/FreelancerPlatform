using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.ServiceForFreelancer
{
    public class ServiceForFreelancerInfor
    {
        public string Name { get; set; }
        public int Fee { get; set; }
    }
}
