using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface ITransactionService
    {
        Task<ServiceResult> CreateTransaction(TransactionCreateRequest request);
        Task<List<TransactionQuickViewModel>> GetTransactionOfContract(int contractId);
        Task<List<TransactionQuickViewModelSecond>> GetAllTransaction();
        Task<ServiceResult> UpdateStatusTransaction(int transactionId, bool status);
    }
}
