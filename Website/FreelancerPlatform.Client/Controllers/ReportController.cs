using Azure;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Report;
using FreelancerPlatform.Application.Extendsions;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform.Client.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport(ReportCreateRequest request)
        {
            request.UserReport = User.GetUserId();
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
            var response = await _reportService.CreateReportAsync(request);
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
