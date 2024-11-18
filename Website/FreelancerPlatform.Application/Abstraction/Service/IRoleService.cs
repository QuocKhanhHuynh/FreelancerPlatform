
using FreelancerPlatform.Application.Dtos.Apply;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface IRoleService
    {
        Task<ServiceResult> CreateRoleAsync(RoleCreateRequest request);
        Task<ServiceResult> UpdateRoleAsync(int id, RoleUpdateRequest request);
        Task<ServiceResult> DeleteRoleAsync(int id);
        Task<Pagination<RoleQuickViewModel>> GetAllRoleAsync(int pageIndex, int pageTake);
        Task<RoleQuickViewModel> GetRoleAsync(int id);
    }
}
