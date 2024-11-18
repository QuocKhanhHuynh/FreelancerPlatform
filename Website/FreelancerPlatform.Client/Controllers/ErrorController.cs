using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform.Client.Controllers
{
	public class ErrorController : Controller
	{
		[Route("404")]
		public IActionResult PageNotFound()
		{
			return View();
		}
	}
}
