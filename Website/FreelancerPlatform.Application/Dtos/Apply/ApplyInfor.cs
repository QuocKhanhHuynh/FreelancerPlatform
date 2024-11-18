using FreelancerPlatform.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Apply
{
    public class ApplyInfor
    {
        public int? Deal { get; set; }
        public int ExecutionDay { get; set; }
        public string? Introduction { get; set; }
    }
}
