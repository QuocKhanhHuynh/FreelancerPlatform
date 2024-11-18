using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Contract
{
    public class PartnerReviewRequest
    {
        public int Id { get; set; }
        public int Point { get; set; }
        public string Review { get; set; }
    }
}
