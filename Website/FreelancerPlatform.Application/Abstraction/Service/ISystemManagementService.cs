using FreelancerPlatform.Application.Dtos.Account;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Freelancer;
using FreelancerPlatform.Application.Dtos.SystemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface ISystemManagementService
    {
        Task<ServiceResult> RegisterAccountAsync(SystemManagementRegisterRequest request);
        Task<LoginResult> LoginAccountAsync(SystemManagementLoginRequest request);
        Task<List<SystemManagementQuickViewModel>> GetAllAdmin();

        Task<ServiceResult> LockAcountAsync(int adminId);
        Task<ServiceResult> UnLockAcountAsync(int adminId);
        /*Task<ServiceResult> RegisterAccountAsync(SystemManagementRegisterRequest request);
        Task<ServiceResult> UpdatePasswordAsync(int id, PasswordUpdateRequest request);
        Task<ServiceResult> CreateSystemManagementAsync(SystemManagementCreateRequest request);
        Task<ServiceResult> UpdateSystemManagementAsync(int id, SystemManagementUpdateRequest request);
        Task<ServiceResult> UpdateSystemManagementRoleAsync(int id, SystemManagementRoleUpdateRequest request);
        Task<Pagination<SystemManagementQuickViewModel>> GetAllSystemManagementAsync(int pageIndex, int pageTake, string? keywork = null);
        Task<SystemManagementViewModel> GetSystemManagementAsync(int id);
        Task<ServiceResult> DeleteAccountAsync(int id);
        Task<ServiceResult> LockAccountAsync(int id);*/

    }
}
