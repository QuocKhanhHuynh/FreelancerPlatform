using Azure.Core;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Apply;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Extendsions;
using FreelancerPlatform.Application.ServiceImplementions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform.Client.Controllers
{
    public class ApplyController : Controller
    {
        private readonly IApplyService _applyService;
        public ApplyController(IApplyService applyService)
        {
            _applyService = applyService;
        }

        [Authorize]
        public async Task<IActionResult> CreateApply(ApplyCreateRequest request)
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


            var response = await _applyService.CreateApplyAsync(request);
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
            var applyOfFreelancer = await _applyService.GetApplyAsync(response.Result);
            return Ok(applyOfFreelancer);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteApply(int id)
        {
            var response = await _applyService.DeleteApplyAsync(id);
            if (response.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
