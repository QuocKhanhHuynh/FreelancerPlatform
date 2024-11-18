namespace FreelancerPlatform.Client.Models
{
    public class TransactionCreateRequestClient
    {
        public int ContractId { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }  
    }
}
