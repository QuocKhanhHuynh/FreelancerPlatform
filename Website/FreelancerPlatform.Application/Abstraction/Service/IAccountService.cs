using FreelancerPlatform.Application.Dtos.Account;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface IAccountService
    {
        Task<ServiceResult> RegisterAccountAsync(AccountRegisterRequest request);
        Task<ServiceResult> RegisterAccountByGoogleAsync(AccountRegisterByGoogleRequest request);
        Task<ServiceResult> UpdatePasswordAsync(int id, PasswordUpdateRequest request);
        /*Task<ServiceResult> UpdatePasswordAsync(int id, PasswordUpdateRequest request);
        Task<ServiceResult> UpdatePriorityAsync(int id, PriorityUpdateRequest request);
        Task<ServiceResult> VerifyAccountAsync(int id, VerificationRequest request);
        Task<ServiceResult> DeleteAccountAsync(int id);
        Task<ServiceResult> LockAccountAsync(int id);*/
    }
}
