using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Contract
{
    public class ContractViewModel : ContractQuickViewModel
    {
        public int RecruiterId {  get; set; }
        public string RecruiterLastName { get; set; }
        public string RecruiterFirstName { get; set; }
        public string RecruiterImageUrl { get; set; }
        public int PartnerId { get; set; }
        public string PartnerLastName { get; set;}
        public string PartnerFirstName { get; set; }
        public string PartnerImageUrl { get; set; }
        public int PartnerPoint { get; set; }
        public string PartnerReview {  get; set; }
    }
}
