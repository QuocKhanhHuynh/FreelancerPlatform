using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Transaction
{
    public class TransactionQuickViewModel
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDay { get; set; }
        public int FreelancerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string ImageUrl { get; set; }

        public int ContractId { get; set; }
    }
}
