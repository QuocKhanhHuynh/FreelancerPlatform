using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Extendsions
{
	public static class IdentityExtendsion
	{
        /*public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
		{
			var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
				.Claims
				.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
			return int.Parse(claim?.Value);
		}*/

        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            // Lấy claim cuối cùng với type là ClaimTypes.NameIdentifier
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
                .Claims
                .LastOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            // Nếu claim không null, chuyển đổi giá trị của nó sang int, nếu không trả về giá trị mặc định (0)
            return claim != null ? int.Parse(claim.Value) : 0;
        }


        public static string GetFirstName(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
                .Claims
                .SingleOrDefault(x => x.Type == "firstName");
            return claim.Value;
        }
    }
}
