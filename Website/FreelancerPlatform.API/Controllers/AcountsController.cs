using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.SystemManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcountsController : ControllerBase
    {
        private readonly ISystemManagementService _systemManagementService;
        public AcountsController(ISystemManagementService systemManagementService)
        {
            _systemManagementService = systemManagementService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(SystemManagementRegisterRequest request)
        {
            var result = await _systemManagementService.RegisterAccountAsync(request);

            return Ok(result);
        }
    }
}
