using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Skill;

namespace FreelancerPlatform.Application.Dtos.Freelancer
{
    public class FreelancerQuickViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public int? RateHour { get; set; }
        public string About { get; set; }

        public double Point { get; set; }
        public int ReviewQuanlity { get; set; }
        public int ContractQuanlity {  set; get; }
        public bool Status { get; set; }
        public DateTime CreateDay { get; set; }
        public DateTime UpdateDay { get; set; }

        public List<CategoryQuickViewModel> Categories { get; set; }
        public List<SkillQuickViewModel> Skills { get; set; }

    }
}
