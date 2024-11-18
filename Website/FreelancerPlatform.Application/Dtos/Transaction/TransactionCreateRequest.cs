using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Transaction
{
    public class TransactionCreateRequest
    {
        public int Amount { get; set; }
        public string Content { get; set; }
        public int FreelancerId { get; set; }
        public int ContractId { get; set; }
    }
}
