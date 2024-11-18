using FreelancerPlatform.Application.Dtos.Freelancer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Offer
{
    public class OfferQuickViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Fullname { get; set; }
        public int StatusAgree { get; set; }
        public int JobId { get; set; }
        public List<string> CategoryName { get; set; }
        public string Address { get; set; }
    }
}
