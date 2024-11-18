
using FreelancerPlatform.ApiService;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Account;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.Potient;
using FreelancerPlatform.Application.Extendsions;
using FreelancerPlatform.Client.Models;
using FreelancerPlatform.Client.Services;
using FreelancerPlatform.Domain.Constans;
using FreelancerPlatform.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FreelancerPlatform.Client.Controllers
{
    public class FreelancerController : Controller
    {
        private readonly IFreelancerService _freelancerService;
        private readonly ICategoryService _categoryService;
        private readonly ISkillService _skillService;
        private readonly IStorageService _storageService;
        private readonly IPotientSerivice _petientSerivice;
        private readonly IJobService _jobService;
        private readonly IFavoriteJobService _favoriteJobService;
        private readonly IApplyService _applyService;
        private readonly IOfferService _offerService;
        private readonly IContractService _contractService;
        private readonly IPostService _postService;
        private readonly ISystemRecommendationApiService _systemRecommendationApiService;
       
        public FreelancerController(IFreelancerService freelancerService, ICategoryService categoryService, ISkillService skillService, IStorageService storageService,
            IPotientSerivice petientSerivice, IJobService jobService, IFavoriteJobService favoriteJobService, IApplyService applyService, IOfferService offerService
            , IContractService contractService, IPostService postService, ISystemRecommendationApiService systemRecommendationApiService)
        {
            _freelancerService = freelancerService;
            _categoryService = categoryService;
            _skillService = skillService;
            _storageService = storageService;
            _petientSerivice = petientSerivice;
            _jobService = jobService;
            _favoriteJobService = favoriteJobService;
            _applyService = applyService;
            _offerService = offerService;
            _contractService = contractService;
            _postService = postService;
            _systemRecommendationApiService = systemRecommendationApiService;
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var freelancer = await _freelancerService.GetFreelancerAsync(User.GetUserId());
            var reviewOfFreelancer = await _contractService.GetReviewOfFreelancer(User.GetUserId());

            ViewBag.ReviewOfFreelancer = reviewOfFreelancer;
            return View(freelancer);
        }


        public async Task<IActionResult> GetFreelancer(string? keyword = null, int? categoryId = null, int? skillId = null)
        {

            var freelancers = await _freelancerService.GetAllFreelancerAsync();//.Where(x => x.Id != User.GetUserId()).ToList();
            var categories = await _categoryService.GetCategoryAsync();
            var skills = await _skillService.GetSkillAsync();
            ViewBag.PotientOfFreelancer = new List<int>();
            if (User.Identity.IsAuthenticated)
            {
                var potientOfFreelancers = await _petientSerivice.GetPotientFreelancerAsync(User.GetUserId());
                ViewBag.PotientOfFreelancer = potientOfFreelancers;
                freelancers = freelancers.Where(x => x.Id != User.GetUserId()).ToList();
            }
            ViewBag.Categories = categories;
            ViewBag.Skills = skills;
            ViewBag.Keyword = keyword;
            ViewBag.CategoryId = categoryId.GetValueOrDefault();
            ViewBag.SkillId = skillId.GetValueOrDefault();
            return View(freelancers);
        }

        [Authorize]
        public async Task<IActionResult> NavigateHire()
        {
           
            var potients = await _petientSerivice.GetAllPotientFreelancerAsync(User.GetUserId());//.Where(x => x.Id != User.GetUserId()).ToList();
            var jobPosts = (await _jobService.GetAllJobsAsync()).Where(x => x.FreelancerId == User.GetUserId()).ToList();
            var offers = (await _offerService.GetAllOffer()).Where(x => x.RecruiterId == User.GetUserId()).ToList();
            var contracts = await _contractService.GetContractOfRecruiter(User.GetUserId());


            var navigateHires = new NavigateHireViewModel() { Potients = potients, JobPosts = jobPosts, OfferJobs = offers, Contracts = contracts };
            
            

            return View(navigateHires);
        }

        [Authorize]
        public async Task<IActionResult> NavigateFreelancer()
        {

            var favoriteJobs = await _favoriteJobService.GetFavoriteJobOfFreelancerSecondAsync(User.GetUserId());
            var jobApplies = (await _applyService.GetApplyByFreelancerAsync(User.GetUserId()));
            var offers = (await _offerService.GetAllOffer()).Where(x => x.FreelancerId == User.GetUserId()).ToList();
            var contracts = await _contractService.GetContractOfFreelancer(User.GetUserId());
            var posts = await _postService.GetPostByFreelancerId(User.GetUserId());
            var savePost = await _postService.GetSavePostByFreelancerId(User.GetUserId());

            var navigateFreelancers = new NavigateFreelancerViewModel() { FavoriteJobs = favoriteJobs, JobApplies = jobApplies, OfferJobs = offers, Contracts = contracts, Posts = posts, SavePosts = savePost };
            var FavoriteJobOfFreelancers = await _favoriteJobService.GetFavoriteJobOfFreelancerAsync(User.GetUserId());
            ViewBag.FavoriteJobOfFreelancers = FavoriteJobOfFreelancers;



            return View(navigateFreelancers);
        }

        [HttpPost]

        public async Task<IActionResult> HidenJob(int id)
        {
            var response = await _jobService.HidenJobAsync(id);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]

        public async Task<IActionResult> RemoveHidenJob(int id)
        {
            var response = await _jobService.DeleteHidenJobAsync(id);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }



        [HttpPost]
        
        public async Task<IActionResult> AddFreelancerPotient(PotientInfor request)
        {
            request.FreelancerId = User.GetUserId();
            var response = await _petientSerivice.AddPotientFreelancerAsync(request);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }



        [HttpPost]
        public async Task<IActionResult> MinusFreelancerPotient(PotientInfor request)
        {
            request.FreelancerId = User.GetUserId();
            var response = await _petientSerivice.RemovePotientFreelancerAsync(request);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            var response = await _offerService.DeleteOfferAsync(id);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        public async Task<IActionResult> GetFreelancerDetail(int id)
        {
            var freelancer = await _freelancerService.GetFreelancerAsync(id);

            var jobOfContract = (await _contractService.GetContractOfRecruiter(User.GetUserId())).Select(x => x.ProjectId);
            var jobs = (await _jobService.GetAllJobsAsync()).Where(x => !jobOfContract.Contains(x.Id)).ToList();
            var JobOffFreelancer = jobs.Where(x => x.FreelancerId == User.GetUserId());
            var reviewOfFreelancer = await _contractService.GetReviewOfFreelancer(id);
            var freelancerIdRecommend = await _systemRecommendationApiService.GetFreelancerRecommendation(id);
            var freelancerRecommendation = new List<FreelancerQuickViewModel>();
            foreach(var i in freelancerIdRecommend)
            {
                var freelancerRecommend = await _freelancerService.GetFreelancerAsync(i);
                freelancerRecommendation.Add(new FreelancerQuickViewModel()
                {
                    Id = freelancerRecommend.Id,
                    FirstName = freelancerRecommend.FirstName,
                    LastName = freelancerRecommend.LastName,
                });
            }
            ViewBag.ReviewOfFreelancer = reviewOfFreelancer;
            ViewBag.JobOfFreelancer = JobOffFreelancer;
            ViewBag.PotientOfFreelancer = new List<int>();
            ViewBag.FreelancerRecommendation = freelancerRecommendation;
            if (User.Identity.IsAuthenticated)
            {
                var potientOfFreelancers = await _petientSerivice.GetPotientFreelancerAsync(User.GetUserId());
                ViewBag.PotientOfFreelancer = potientOfFreelancers;
            }
            return View(freelancer);
        }

        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var freelancer = await _freelancerService.GetFreelancerAsync(User.GetUserId());
            var categories = await _categoryService.GetCategoryAsync();
            var skills = await _skillService.GetSkillAsync();
            ViewBag.CategoryJson = JsonConvert.SerializeObject(categories);
            ViewBag.SkillJson = JsonConvert.SerializeObject(skills);
            ViewBag.Categories = categories;
            ViewBag.Skills = skills;
            ViewBag.CategoryOfFreelancer = freelancer.Categories;
            ViewBag.SkillOfFreelancer = freelancer.Skills;
            ViewBag.CategoryOfFreelancerJSon = JsonConvert.SerializeObject(freelancer.Categories);
            ViewBag.SkillOfFreelancerJson = JsonConvert.SerializeObject(freelancer.Skills);
            var request = new FreelancerUpdateRequest()
            {
                About = freelancer.About,
                FirstName = freelancer.FirstName,
                LastName = freelancer.LastName,
                ImageUrl = freelancer.ImageUrl,
                RateHour = freelancer.RateHour,
                CategoryIds = freelancer.Categories.Select(x => x.Id).ToList(),
                SkillIds = freelancer.Skills.Select(x => x.Id).ToList(),
                Experience = freelancer.Experience,
                Education = freelancer.Education,
                Certification = freelancer.Certification,
                Archive = freelancer.Archive
            };
            return View(request);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [Authorize]
        public async Task<IActionResult> EditProfile([FromForm] FormFreelancerUpdateRequest request)
        {
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

            var applicationRequest = new FreelancerUpdateRequest()
            {
                About = request.About,
                FirstName = request.FirstName,
                LastName = request.LastName,
                CategoryIds = request.CategoryIds,
                SkillIds = request.SkillIds,
                RateHour = request.RateHour,
                Archive = request.Archive,
                Certification = request.Certification,
                Education = request.Education,
                Experience = request.Experience,
            };
            if (request.ImageUrl != null)
            {
                var freelancer = await _freelancerService.GetFreelancerAsync(User.GetUserId());
                if (!string.IsNullOrEmpty(freelancer.ImageUrl))
                    await _storageService.DeleteFileAsync(freelancer.ImageUrl);
                applicationRequest.ImageUrl = await SaveFileAsync(request.ImageUrl);
            }

            var response = await _freelancerService.UpdateFreelancerAsync(User.GetUserId(), applicationRequest);
            return Ok(request);
        }

        [Authorize]
        public async Task<IActionResult> EditPayment()
        {
            var freelancer = await _freelancerService.GetFreelancerAsync(User.GetUserId());
            var banks = Constans.Bank;
            ViewBag.Banks = banks;
            return View(freelancer);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPayment(FreelancerPaymentUpdateRequest request)
        {
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


            var response = await _freelancerService.UpdatePaymentAsync(User.GetUserId(), request);
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

            return Ok(response.Message);
        }

      


        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = file.FileName; //$"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
