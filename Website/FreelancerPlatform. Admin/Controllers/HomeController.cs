using FreelancerPlatform._Admin.Models;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FreelancerPlatform._Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IFreelancerService _freelancerService;
        private readonly ITransactionService _transactionService;
        private readonly IContractService _contractService;
       
        private readonly ILogger<HomeController> _logger;
        public HomeController(IFreelancerService freelancerService, ITransactionService transactionService, IContractService contractService, ILogger<HomeController> logger)
        {
            _freelancerService = freelancerService;
            _transactionService = transactionService;
            _contractService = contractService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var freelancers = await _freelancerService.GetAllFreelancerAsync();
            var contracts = await _contractService.GetAllContract();
            var transactions = await _transactionService.GetAllTransaction();

            var result = new HomeViewModel()
            {
                AcountNumber = freelancers.Count,
                AsssetNumber = 100,
                ContractNumber = contracts.Count,
                TransactionNumber = transactions.Count,
                Transactions = transactions.Where(x => x.Status == false).ToList()
            };
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
