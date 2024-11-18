using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Offer
{
    public class OfferCreateRequest : OfferInfor
    {
        public int FreelancerId { get; set; }
		public int JobId { get; set; }
        public int FreelancerOfferId { get; set; }
    }
}
