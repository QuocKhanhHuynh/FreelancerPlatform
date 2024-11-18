using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreelancerPlatform.Domain.Entity;

namespace FreelancerPlatform.Application.Dtos.Job
{
    public class JobInfor
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinDeal { get; set; }
        public int? MaxDeal { get; set; }
        public int CategoryId { get; set; }
        public int FreelancerId { get; set; }
        public int SalaryType {  get; set; }
        public int WorkType { get; set; }
        public int JobType { get; set; }
        public string? Requirement { get; set; }
        public int? EstimatedCompletion { get; set; }
        public int? HourPerDay { get; set; }
        public List<int>? Skills { get; set; }
    }
}
