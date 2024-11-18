using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Transaction
{
    public class TransactionQuickViewModelSecond
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDay { get; set; }
        public int FreelancerA { get; set; }
        public string LastNameA { get; set; }
        public string FirstNameA { get; set; }

        public int FreelancerB { get; set; }
        public string LastNameB { get; set; }
        public string FirstNameB { get; set; }

        public string BankNameReceipt { get; set; }
        public string BankNumberReceipt { get; set; }

        public int ContractId { get; set; }
    }
}
