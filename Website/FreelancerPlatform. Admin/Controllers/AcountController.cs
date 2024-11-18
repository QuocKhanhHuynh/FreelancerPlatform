using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Account;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Extendsions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FreelancerPlatform.Application.Dtos.SystemManagement;

namespace FreelancerPlatform._Admin.Controllers
{
    public class AcountController : Controller
    {
        private readonly ISystemManagementService _systemManagementService;
        private readonly IFreelancerService _freelancerService;
        private readonly IConfiguration _configuration;
        public AcountController(ISystemManagementService systemManagementService, IConfiguration configuration, IFreelancerService freelancerService)
        {
            _systemManagementService = systemManagementService;
            _configuration = configuration;
            _freelancerService = freelancerService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SystemManagementLoginRequest request)
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
            var result = await _systemManagementService.LoginAccountAsync(request);
            if (result.Status != StatusResult.Success)
            {
                var errors = new List<object>
                {
                    new
                    {
                        Field = "ResultStatus",
                        Errors = new[] { result.Message }
                    }
                };

                return BadRequest(new { Errors = errors });
            }

            var userPrincial = this.ValidateToken(result.Token);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60),
                IsPersistent = false
            };
            HttpContext.Session.SetString("AdminToken", result.Token);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincial, authProperties);

            return Ok(result.Message);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(SystemManagementRegisterRequest request)
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
            var result = await _systemManagementService.RegisterAccountAsync(request);
            if (result.Status != StatusResult.Success)
            {
                var errors = new List<object>
                {
                    new
                    {
                        Field = "ResultStatus",
                        Errors = new[] { result.Message }
                    }
                };

                return BadRequest(new { Errors = errors });
            }

            return Ok(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("AdminToken");
            return Redirect("/");
        }

        public async Task<IActionResult> GetAdmin()
        {
            var admins = await _systemManagementService.GetAllAdmin();
            return View(admins);
        }

        public async Task<IActionResult> GetFreelancer(string keyword = null)
        {
            var freelancers = await _freelancerService.GetAllFreelancerAsync();
            if (!string.IsNullOrEmpty(keyword))
            {
                
                freelancers = freelancers.Where(x => $"{x.LastName.ToUpper()} {x.FirstName.ToUpper()}".Contains(keyword.Trim()) || x.Id.ToString() == keyword.Trim()).ToList();
            }
            return View(freelancers);
        }

        public async Task<IActionResult> CreateAdmin()
        {
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Lock(int adminId)
        {
            
            var result = await _systemManagementService.LockAcountAsync(adminId);
            
            if (result.Status != StatusResult.Success)
            {

            return BadRequest(result.Message); 
            }
            return Ok(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> UnLock(int adminId)
        {

            var result = await _systemManagementService.UnLockAcountAsync(adminId);

            if (result.Status != StatusResult.Success)
            {

                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> LockFreelancer(int freelancerId)
        {

            var result = await _freelancerService.LockAcountAsync(freelancerId);

            if (result.Status != StatusResult.Success)
            {

                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> UnLockFreelancer (int freelancerId)
        {

            var result = await _freelancerService.UnLockAcountAsync(freelancerId);

            if (result.Status != StatusResult.Success)
            {

                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        public async Task<IActionResult> GetVerifyPayment()
        {
            var result = await _freelancerService.GetFeelancerVerifyPayment();
            return View(result);
        }

        public async Task<IActionResult> GetVerifyPaymentDetail(int id)
        {
            var result = (await _freelancerService.GetFeelancerVerifyPayment()).FirstOrDefault(x => x.Id == id);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVerifyPayment(int id, bool status)
        {

            var result = await _freelancerService.UpdateVerifyPayment(id, status);

            if (result.Status != StatusResult.Success)
            {

                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        /*public IActionResult EditPassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPassword(PasswordUpdateRequest request)
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


            var response = await _systemManagementService.UpdatePasswordAsync(User.GetUserId(), request);
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
        }*/



        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["Tokens:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Tokens:Issuer"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])),
            };
            return new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
        }
    }
}
