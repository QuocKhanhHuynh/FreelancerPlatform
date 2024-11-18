using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform._Admin.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public async Task<IActionResult> GetReport(string keyword = null)
        {

            var reports = await _reportService.GetReportAllAsync();
            if (!string.IsNullOrEmpty(keyword))
            {
                reports = reports.Where(x => $"{x.LastName.ToUpper()} {x.FirstName.ToUpper()}".Contains(keyword.Trim()) || x.Id.ToString() == keyword.Trim()).ToList();
            }
            return View(reports);
        }

        public async Task<IActionResult> GetReportDetail(int id)
        {
            var report = await _reportService.GetReportAsync(id);
            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> ReviewReport(int id)
        {
            var resutl = await _reportService.ReviewReportAsync(id);

            if (resutl.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UnReviewReport(int id)
        {
            var resutl = await _reportService.UnReviewReportAsync(id);

            if (resutl.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
