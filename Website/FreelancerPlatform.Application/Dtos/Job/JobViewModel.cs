using FreelancerPlatform.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Skill;

namespace FreelancerPlatform.Application.Dtos.Job
{
    public class JobViewModel : JobQuickViewModel
    {
        public string Requirement { get; set; }
        public int? EstimatedCompletion { get; set; }
        public int? HourPerDay { get; set; }
    }
}
