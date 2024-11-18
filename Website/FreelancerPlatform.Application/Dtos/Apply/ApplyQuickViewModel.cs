using FreelancerPlatform.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreelancerPlatform.Application.Dtos.Job;

namespace FreelancerPlatform.Application.Dtos.Apply
{
    public class ApplyQuickViewModel : JobQuickViewModel
    {
        public int ApplyId { get; set; }
        public int Deal { get; set; }
        public int ExecutionDay { get; set; }
        public int Status { get; set; }
        public int JobId { get; set; }
    }
}
