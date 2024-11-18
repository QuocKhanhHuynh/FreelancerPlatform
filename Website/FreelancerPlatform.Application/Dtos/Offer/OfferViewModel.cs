using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreelancerPlatform.Application.Dtos.Freelancer;

namespace FreelancerPlatform.Application.Dtos.Offer
{
    public class OfferViewModel : FreelancerViewModel
    {
        public int OfferId { get; set; }
        public int StatusAgree { get; set; }
        public int JobId { get; set; }
        public string JobName { get; set; }
    }
}
