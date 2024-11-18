using FreelancerPlatform.Application.Dtos.Contract;
using FreelancerPlatform.Application.Dtos.Job;
using FreelancerPlatform.Application.Dtos.Offer;
using FreelancerPlatform.Application.Dtos.Potient;

namespace FreelancerPlatform.Client.Models
{
    public class NavigateHireViewModel
    {
        public List<PotientQuickViewModel> Potients { get; set; }
        public List<JobQuickViewModel> JobPosts { get; set; }
        public List<OfferQuicckViewModel> OfferJobs { get; set; }
        public List<ContractQuickViewModel> Contracts { get; set; }
    }
}
