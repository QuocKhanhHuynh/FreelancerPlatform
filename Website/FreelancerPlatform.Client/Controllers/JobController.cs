using FreelancerPlatform.ApiService;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.FavoriteJob;
using FreelancerPlatform.Application.Dtos.Job;
using FreelancerPlatform.Application.Dtos.Offer;
using FreelancerPlatform.Application.Dtos.Potient;
using FreelancerPlatform.Application.Extendsions;
using FreelancerPlatform.Application.ServiceImplementions;
using FreelancerPlatform.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;

namespace FreelancerPlatform.Client.Controllers
{
    public class JobController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ISkillService _skillService;
        private readonly IJobService _jobService;
        private readonly IFavoriteJobService _favoriteJobService;
        private readonly IFreelancerService _freelancerService;
        private readonly IApplyService _applyService;
        private readonly IOfferService _offerService;
        private readonly ISystemRecommendationApiService _systemRecommendationApiService;

        public JobController(ICategoryService categoryService, ISkillService skillService, IJobService jobService, ISystemRecommendationApiService systemRecommendationApiService,
            IFavoriteJobService favoriteJobService, IFreelancerService freelancerService, IApplyService applyService, IOfferService offerService)
        {
            _categoryService = categoryService;
            _skillService = skillService;
            _jobService = jobService;
            _favoriteJobService = favoriteJobService;
            _freelancerService = freelancerService;
            _applyService = applyService;
            _offerService = offerService;
            _systemRecommendationApiService = systemRecommendationApiService;
        }

        public async Task<IActionResult> PostJob()
        {

            var categories = await _categoryService.GetCategoryAsync();
            var skills = await _skillService.GetSkillAsync();
            ViewBag.Categories = categories;
            ViewBag.Skills = skills;
            ViewBag.SkillJson = JsonConvert.SerializeObject(skills);


            return View();
        }

        public async Task<IActionResult> EditJob(int id)
        {

            var job = await _jobService.GetJobAsync(id);
            var request = new JobUpdateRequest()
            {
                CategoryId = job.Category.Id,
                Description = job.Description,
                FreelancerId = job.FreelancerId,
                JobType = job.JobType,
                MaxDeal = job.MaxDeal,
                MinDeal = job.MinDeal,
                Name = job.Name,
                SalaryType = job.SalaryType,
                Skills = job.Skills.Select(x => x.Id).ToList(),
                Id = job.Id,
                Requirement = job.Requirement,
                HourPerDay = job.HourPerDay,
                EstimatedCompletion = job.EstimatedCompletion,
                WorkType = job.WorkType
            };

            var categories = await _categoryService.GetCategoryAsync();
            var skills = await _skillService.GetSkillAsync();
            var skillOfJob = job.Skills;
            ViewBag.Categories = categories;
            ViewBag.Skills = skills;
            ViewBag.SkillOfJob = skillOfJob;
            ViewBag.SkillJson = JsonConvert.SerializeObject(skills);

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> EditJob(JobUpdateRequest request)
        {

            request.FreelancerId = User.GetUserId();
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                })
                .ToList();

                return BadRequest(new { Errors = errors });
            }


            var response = await _jobService.UpdateJobAsync(request.Id, request);
            if (response.Status != StatusResult.Success)
            {
                var errors = new List<object>
                {
                    new
                    {
                        Field = "ResultStatus",
                        Errors = new[] { response.Message }
                    }
                };

                return BadRequest(new { Errors = errors });
            }
         //   await _systemRecommendationApiService.BuildSystemRecommendation();
            return Ok(response.Message);

        }

        public async Task<IActionResult> GetJob(string? keyword = null, int? categoryId = null, int? skillId = null)
        {
            var jobs = (await _jobService.GetAllJobsAsync()).Where(x => x.IsHiden == false).ToList();

            var categories = await _categoryService.GetCategoryAsync();
            var skills = await _skillService.GetSkillAsync();
            ViewBag.FavoriteJobOfFreelancers = new List<int>();
            if (User.Identity.IsAuthenticated)
            {
                var userId = (User.GetUserId());
                var FavoriteJobOfFreelancers = await _favoriteJobService.GetFavoriteJobOfFreelancerAsync(userId);
                ViewBag.FavoriteJobOfFreelancers = FavoriteJobOfFreelancers;
                jobs = jobs.Where(x => x.FreelancerId != User.GetUserId()).ToList();
            }
            ViewBag.Categories = categories;
            ViewBag.Skills = skills;
            ViewBag.Keyword = keyword;
            ViewBag.CategoryId = categoryId.GetValueOrDefault();
            ViewBag.SkillId = skillId.GetValueOrDefault();

            return View(jobs);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddFavoriteJob(FavoriteJobCreateRequest request)
        {
            request.FreelancerId = User.GetUserId();
            var response = await _favoriteJobService.CreateFavoriteJobAsync(request);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MinusFavoriteJob(FavoriteJobDeleteRequest request)
        {
            request.FreelancerId = User.GetUserId();
            var response = await _favoriteJobService.DeleteFavoriteJobAsync(request);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddOfferJob(OfferCreateRequest request)
        {
            request.FreelancerId = User.GetUserId();
            var response = await _offerService.CreateOfferAsync(request);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PostJob(JobCreateRequest request)
        {
            request.FreelancerId = User.GetUserId();
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new
                {
                    Field = x.Key,
                    Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                })
                .ToList();

                return BadRequest(new { Errors = errors });
            }


            var response = await _jobService.CreateJobAsync(request);
            if (response.Status != StatusResult.Success)
            {
                var errors = new List<object>
                {
                    new
                    {
                        Field = "ResultStatus",
                        Errors = new[] { response.Message }
                    }
                };

                return BadRequest(new { Errors = errors });
            }
           // await _systemRecommendationApiService.BuildSystemRecommendation();

            return Ok(response.Message);
        }


        public async Task<IActionResult> GetJobDetail(int id)
        {
            var jobDetail = await _jobService.GetJobAsync(id);
            var freelancer = await _freelancerService.GetFreelancerAsync(jobDetail.FreelancerId);
            var jobs = await _jobService.GetAllJobsAsync();
            var relatedJobs = await this.GetRecommendJob(id); //jobs.Where(x => x.Category.Id == jobDetail.Category.Id && x.Id != jobDetail.Id);
            var applyOfJob = await _applyService.GetApplyOfJobAsync(id);
            ViewBag.IsOffer = false;
            if (User.Identity.IsAuthenticated)
            {
                var isOffer = await _offerService.CheckIsOffer(id, User.GetUserId());
                ViewBag.IsOffer = isOffer;
            }

            ViewBag.Freelancer = freelancer;
            ViewBag.RelatedJob = relatedJobs;
            ViewBag.ApplyOfJob = applyOfJob;
            ViewBag.CreateUserName = $"{freelancer.LastName} + {freelancer.FirstName}";

            if (User.Identity.IsAuthenticated && jobDetail.FreelancerId != User.GetUserId())
            {
                await _jobService.AddViewRecent(User.GetUserId(), id);
            }
           


            return View(jobDetail);
        }


        private async Task<List<JobQuickViewModel>> GetRecommendJob(int id)
        {
            var recommendIdJobs = await _systemRecommendationApiService.GetRecommendation(id);
            var recommendjobList = new List<JobQuickViewModel>();
            foreach (var j in recommendIdJobs)
            {
                var job = await _jobService.GetJobAsync(j);
                recommendjobList.Add(new JobQuickViewModel()
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

            return recommendjobList;
        }
    }
}
