using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Contract;
using FreelancerPlatform.Application.Extendsions;
using FreelancerPlatform.Application.ServiceImplementions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform.Client.Controllers
{
    [Authorize]
    public class ContractController : Controller
    {
        private readonly IContractService _contractService;
        private readonly IFreelancerService _freelancerService;
        private readonly ITransactionService _transactionService;
        private readonly IJobService _jobService;

        public ContractController(IContractService contractService, IFreelancerService freelancerService, ITransactionService transactionService, IJobService jobService)
        {
            _contractService = contractService;
            _freelancerService = freelancerService;
            _transactionService = transactionService;
            _jobService = jobService;
         
        }

        public async Task<IActionResult> CreateContract(int createUser, int partner)
        {
            var CreateUser = await _freelancerService.GetFreelancerAsync(createUser);
            var Partner = await _freelancerService.GetFreelancerAsync(partner);
            
            var ProjectOfContract = (await _contractService.GetContractOfRecruiter(User.GetUserId())).Select(x => x.ProjectId).ToList();
            var JobOfUser = (await _jobService.GetAllJobsAsync()).Where(x => x.FreelancerId == User.GetUserId() && !ProjectOfContract.Contains(x.Id)).ToList();

            ViewBag.CreateUserFullName = $"{CreateUser.LastName} {CreateUser.FirstName}";
            ViewBag.PartnerFullName = $"{Partner.LastName} {Partner.FirstName}";

            ViewBag.CreateUser = createUser;
            ViewBag.Partner = partner;

            ViewBag.JobOfUser = JobOfUser;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetContenContract(int projectId)
        {


            var response = await _jobService.GetJobAsync(projectId);
            

            return Ok(response.Name);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContract(ContractCreateRequest request)
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


            var response = await _contractService.CreateContract(request);
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

        public async Task<IActionResult> GetContract(int id)
        {
            var contract = await _contractService.GetContract(id);
            var transactions = await _transactionService.GetTransactionOfContract(id);

            var freelancer = await _freelancerService.GetFreelancerAsync(contract.PartnerId);
            var recruiter = await _freelancerService.GetFreelancerAsync(contract.RecruiterId);

            ViewBag.Freelancer = freelancer;
            ViewBag.Recruiter = recruiter;
            ViewBag.Transaction = transactions;

            return View(contract);
        }

        [HttpPost]

        public async Task<IActionResult> UpdateContractAcceptStatus(int contractId, int status)
        {
            var response = await _contractService.UpdateContractAcceptStatus(contractId, status);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]

        public async Task<IActionResult> UpdateContractStatus(int contractId)
        {
            var response = await _contractService.UpdateContractStatus(contractId);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        public async Task<IActionResult> UpdateContract(int id)
        {
            var contract = await _contractService.GetContract(id);
            var CreateUser = await _freelancerService.GetFreelancerAsync(contract.RecruiterId);
            var Partner = await _freelancerService.GetFreelancerAsync(contract.PartnerId);
            ViewBag.CreateUserFullName = $"{CreateUser.LastName} {CreateUser.FirstName}";
            ViewBag.PartnerFullName = $"{Partner.LastName} {Partner.FirstName}";
            var request = new ContractUpdateRequest()
            {
                Id = id,
                Content = contract.Content,
                Name = contract.Name,
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContract(ContractUpdateRequest request)
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


            var response = await _contractService.UpdateContract(request);
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

        [HttpPost]
        public async Task<IActionResult> ReviewPartner(PartnerReviewRequest request)
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


            var response = await _contractService.ReviewPartner(request);
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
    }
}
