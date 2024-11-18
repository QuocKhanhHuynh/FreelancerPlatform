using FreelancerPlatform.Application.Dtos.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Offer
{
    public class OfferQuicckViewModel : JobQuickViewModel
    {
        public int OfferId { get; set; }
        public int FreelancerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string ImageUrl { get; set; }
        public int RecruiterId { get; set; }
    }
}
