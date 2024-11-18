using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Job
{
    public class JobQuickViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryQuickViewModel Category { get; set; }
        public int FreelancerId { get; set; }
        public DateTime CreateDay { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int MinDeal { get; set; }
        public int MaxDeal { get; set; }
        public int JobType { get; set; }
        public int SalaryType { get; set; }
        public int WorkType { get; set; }
        public List<SkillQuickViewModel> Skills { get; set; }
        public bool IsHiden { get; set; }

        public bool InContract { get; set; }
    }
}
