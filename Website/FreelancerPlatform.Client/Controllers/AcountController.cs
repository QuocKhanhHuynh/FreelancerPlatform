using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Account;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Login;
using FreelancerPlatform.Application.ServiceImplementions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Extendsions;
using FreelancerPlatform.Client.Models;
using FreelancerPlatform.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Google;
using System.Text.RegularExpressions;

namespace FreelancerPlatform.Client.Controllers
{
    public class AcountController : Controller
    {
        private readonly IFreelancerService _freelancerService;
        private readonly IConfiguration _configuration;
        public AcountController(IFreelancerService freelancerService, IConfiguration configuration)
        {
            _freelancerService = freelancerService;
            _configuration = configuration;
        }

        public async Task LoginByGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
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
            var result = await _freelancerService.LoginAsync(request);
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
            HttpContext.Session.SetString("FreelancerToken", result.Token);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincial, authProperties);

            return Ok(result.Message);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterRequest request)
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
            var result = await _freelancerService.RegisterAccountAsync(request);
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
            HttpContext.Session.Remove("FreelancerToken");
            return Redirect("/");
        }

        public IActionResult EditPassword()
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


            var response = await _freelancerService.UpdatePasswordAsync(User.GetUserId(), request);
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

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            /*var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value
            });*/
            var claimsIdentity = result.Principal.Identities.FirstOrDefault();
            if (claimsIdentity != null)
            {
               
                var emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);
                var firstNameClaim = result.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");
                var lastNameClaim = result.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");
                var email = emailClaim.Value;
               // return Json(lastNameClaim.Value);
                var checkUser = await _freelancerService.CheckExistFreelancerByEmail(email);
                if (!checkUser)
                {
                    await _freelancerService.RegisterAccountByGoogleAsync(new AccountRegisterByGoogleRequest()
                    {
                        Email = email,
                        FirstName = Regex.Unescape(firstNameClaim.Value),
                        LastName = Regex.Unescape(lastNameClaim.Value)
                    });
                }
                var freelancer = await _freelancerService.GetFreelancerByEmailAsync(email);
              
                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, $"{freelancer.Id}"));
                claimsIdentity.AddClaim(new Claim("firstName", freelancer.FirstName));
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            }
            return Redirect("/");
        }
    }
}
