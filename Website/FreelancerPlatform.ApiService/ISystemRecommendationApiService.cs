using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.ApiService
{
    public interface ISystemRecommendationApiService
    {
        Task BuildSystemRecommendation();
        Task<List<int>> GetRecommendation(int jobId);
        Task<List<int>> GetFreelancerRecommendation(int freelancerId);
    }
}
