using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Contract
{
    public class ReviewOfFreelancerQuickViewModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime CreateDay { get; set; }
        public string Review { get; set; }
        public int Point { get; set; }
        public string ImageUrl { get; set; }
    }
}
