using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Dtos.Common;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform._Admin.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        public async Task<IActionResult> GetTransaction(string keyword = null, bool? status = null)
        {
            
            var transactions = await _transactionService.GetAllTransaction();
            if (!string.IsNullOrEmpty(keyword))
            {
                transactions = transactions.Where(x => x.Id.ToString()== keyword ).ToList();
            }
            if (status != null)
            {
                transactions = transactions.Where(x => x.Status == status.GetValueOrDefault()).ToList();
            }
            return View(transactions);
        }

        public async Task<IActionResult> GetTransactionDetail(int id)
        {

            var transactions = await _transactionService.GetAllTransaction();
            var transaction = transactions.FirstOrDefault(x => x.Id == id); 
            return View(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, bool status)
        {

            var result = await _transactionService.UpdateStatusTransaction(id, status);
            if (result.Status != StatusResult.Success)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
