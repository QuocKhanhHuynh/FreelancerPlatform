using System.Diagnostics.Contracts;

namespace FreelancerPlatform.Client.Models
{
    public class VnPayRequestModel
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }   
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ContractId { get; set; }
    }
}
