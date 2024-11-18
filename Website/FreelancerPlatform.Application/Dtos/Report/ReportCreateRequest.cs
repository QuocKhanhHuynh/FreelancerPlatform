using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Report
{
    public class ReportCreateRequest
    {
        public string Content {  get; set; }
        public int FreelancerId { get; set; }
        public int UserReport {  get; set; }
    }
}
