using AutoMapper;
using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Application.Abstraction.Service;
using FreelancerPlatform.Application.Abstraction.UnitOfWork;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Transaction;
using FreelancerPlatform.Application.ServiceImplementions.Base;
using FreelancerPlatform.Domain.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.ServiceImplementions
{
    public class TransactionService : BaseService<Transaction>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IFreelancerRepository _freelancerRepository;
        private readonly IContractRepository _contractRepository;
        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<Transaction> logger, 
            ITransactionRepository transactionRepository, IFreelancerRepository freelancerRepository, IContractRepository contractRepository) : base(unitOfWork, mapper, logger)
        {
            _transactionRepository = transactionRepository;
            _freelancerRepository = freelancerRepository;
            _contractRepository = contractRepository;
        }
        public async Task<ServiceResult> CreateTransaction(TransactionCreateRequest request)
        {
            try
            {
                var transaction = new Transaction()
                {
                    Amount = request.Amount,
                    ContractId = request.ContractId,
                    Content = request.Content,
                    FreelancerId = request.FreelancerId
                };
                await _transactionRepository.CreateAsync(transaction);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Tạo thông tin thành công",
                    Status = StatusResult.Success,
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResultInt()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }

        public async Task<List<TransactionQuickViewModelSecond>> GetAllTransaction()
        {
            var transactions = await _transactionRepository.GetAllAsync();
            var freelancers = await _freelancerRepository.GetAllAsync();
            var contracts = await _contractRepository.GetAllAsync();

            var result = new List<TransactionQuickViewModelSecond>();
            foreach ( var transaction in transactions)
            {
                var contract = contracts.FirstOrDefault(x => x.Id == transaction.ContractId);
                var freelancerA = freelancers.FirstOrDefault(x => x.Id == contract.CreateUser);
                var freelancerB = freelancers.FirstOrDefault(x => x.Id == contract.Partner);

                result.Add(new TransactionQuickViewModelSecond()
                {
                    Amount = transaction.Amount,
                    ContractId = transaction.ContractId,
                    Content = transaction.Content,
                    Id = transaction.Id,
                    Status = transaction.Status,
                    CreateDay = transaction.CreateDay,
                    FreelancerA = freelancerA.Id,
                    FirstNameA = freelancerA.FirstName,
                    FreelancerB = freelancerB.Id,
                    FirstNameB = freelancerB.FirstName,
                    LastNameA = freelancerA.LastName,
                    LastNameB = freelancerB.LastName,
                    BankNameReceipt = freelancerB.BankName,
                    BankNumberReceipt = freelancerB.BankNumber
                    
                });
            }
            return result;
        }

        public async Task<List<TransactionQuickViewModel>> GetTransactionOfContract(int contractId)
        {
            var transactions = await _transactionRepository.GetAllAsync();
            var freelancer = await _freelancerRepository.GetAllAsync();
            var query = from t in transactions
                       join f in freelancer on t.FreelancerId equals f.Id
                       select new {t, f};

            return query.Where(x => x.t.ContractId == contractId).Select(x => new TransactionQuickViewModel()
            {
                Amount = x.t.Amount,
                ContractId=x.t.ContractId,
                Content = x.t.Content,
                Id = x.t.Id,
                Status = x.t.Status,
                CreateDay = x.t.CreateDay,
                LastName = x.f.LastName,
                FirstName = x.f.FirstName,
                FreelancerId = x.f.Id,
                ImageUrl = x.f.ImageUrl
            }).ToList();
        }

        public async Task<ServiceResult> UpdateStatusTransaction(int transactionId, bool status)
        {
            try
            {
                var transaction = await _transactionRepository.GetByIdAsync(transactionId);
                transaction.Status = status;
                _transactionRepository.Update(transaction);
                await _unitOfWork.SaveChangeAsync();

                return new ServiceResult()
                {
                    Message = "Cập nhật thông tin thành công",
                    Status = StatusResult.Success,
                };
            }
            catch (Exception ex)
            {
                _unitOfWork.Cancel();
                _logger.LogError(ex.InnerException.Message);

                return new ServiceResultInt()
                {
                    Message = $"Lỗi hệ thống, ({ex.InnerException.Message})",
                    Status = StatusResult.SystemError
                };
            }
        }
    }
}
