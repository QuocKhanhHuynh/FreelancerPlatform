using AutoMapper.Configuration.Conventions;
using FreelancerPlatform.Application.Dtos.Freelancer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Potient
{
    public class OfferQuickViewModel : FreelancerQuickViewModel
    {
        public int Status { get; set; }
    }
}
