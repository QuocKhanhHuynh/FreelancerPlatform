using FreelancerPlatform.Application.Dtos.Contract;
using FreelancerPlatform.Application.Dtos.Job;
using FreelancerPlatform.Application.Dtos.Offer;
using FreelancerPlatform.Application.Dtos.Post;
using FreelancerPlatform.Application.Dtos.Potient;

namespace FreelancerPlatform.Client.Models
{
    public class NavigateFreelancerViewModel
    {
        public List<JobQuickViewModel> FavoriteJobs { get; set; }
        public List<JobQuickViewModel> JobApplies { get; set; }
        public List<OfferQuicckViewModel> OfferJobs { get; set; }
        public List<ContractQuickViewModel> Contracts { get; set; }
        public List<PostViewModel> Posts { get; set; }
        public List<PostViewModel> SavePosts { get; set; }
    }
}
