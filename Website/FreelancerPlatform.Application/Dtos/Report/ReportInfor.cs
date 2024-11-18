using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Report
{
    public class ReportInfor
    {
        public string Content { get; set; }
        public int ReportType { get; set; }
        public int RecruiterId { get; set; }
        public int FreelancerId { get; set; }

    }
}
