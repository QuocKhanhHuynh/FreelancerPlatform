using FreelancerPlatform.ApiService;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Job;
using FreelancerPlatform.Application.Extendsions;
using FreelancerPlatform.Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FreelancerPlatform.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IJobService _jobService;
        private readonly ISystemRecommendationApiService _systemRecommendationApiService;
        private readonly IFreelancerService _freelancerService;
        public HomeController(ILogger<HomeController> logger, IJobService jobService, ISystemRecommendationApiService systemRecommendationApiService, IFreelancerService freelancerService)
        {
            _jobService = jobService;
            _logger = logger;
            _systemRecommendationApiService = systemRecommendationApiService;
            _freelancerService = freelancerService;
        }
        public async Task<IActionResult> Index()
        {
            var recentView = await _jobService.GetRecentView();
            var topFreelancer = (await _freelancerService.GetAllFreelancerAsync()).OrderByDescending(x => x.ContractQuanlity).Take(6).ToList();
           
            var result = new HomeViewModel()
            {
                RecentViews = recentView,
                TopFreelancer = topFreelancer
            };
            if (User.Identity.IsAuthenticated)
            {
                var recentViewOfUser = await _jobService.GetJobIdRecentViewOfFreelancer(User.GetUserId());


                if (recentViewOfUser.Count > 0)
                {
                    var recommendjobIdList = new List<JobQuickViewModel>();
                    foreach (var i in recentViewOfUser)
                    {
                       
                        var recommendIdJobs = await _systemRecommendationApiService.GetRecommendation(i);
                        foreach(var j in recommendIdJobs)
                        {
                            if (recentViewOfUser.Contains(j))
                            {
                                continue;
                            }
                            var job = await _jobService.GetJobAsync(j);
                            recommendjobIdList.Add(new JobQuickViewModel()
                            {
                                Category = job.Category,
                                CreateDay = job.CreateDay,
                                Description = job.Description,
                                FreelancerId = job.FreelancerId,
                                Id = job.Id,
                                InContract = job.InContract,
                                IsHiden = job.IsHiden,
                                JobType = job.JobType,
                                MaxDeal = job.MaxDeal,
                                MinDeal = job.MinDeal,
                                Name = job.Name,
                                Priority = job.Priority,
                                SalaryType = job.SalaryType,
                                Skills = job.Skills,
                               
                            });
                        }
                        
                    }
                    result.RecommendJobs = recommendjobIdList.DistinctBy(x => x.Id).ToList();
                }

            }
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
