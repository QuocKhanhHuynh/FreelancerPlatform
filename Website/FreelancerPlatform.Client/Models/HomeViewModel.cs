using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.Job;

namespace FreelancerPlatform.Client.Models
{
    public class HomeViewModel
    {
        public List<JobQuickViewModel> RecentViews { get; set; }
        public List<JobQuickViewModel> RecommendJobs { get; set; }
        public List<FreelancerQuickViewModel> TopFreelancer { get; set; }
    }
}
