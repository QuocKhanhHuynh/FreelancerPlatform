using FreelancerPlatform.Application.Dtos.Transaction;

namespace FreelancerPlatform._Admin.Models
{
    public class HomeViewModel
    {
        public int AcountNumber { get; set; }
        public int AsssetNumber { get; set; }
        public int ContractNumber { get; set; }
        public int TransactionNumber { get; set; }
        public List<TransactionQuickViewModelSecond> Transactions { get; set; }
    }
}
