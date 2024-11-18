using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Report
{
    public class ReportQuickViewModel
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; }
        public int FreelancerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int NumberReport { get; set; }
    }
}
