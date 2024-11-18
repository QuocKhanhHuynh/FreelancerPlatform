using FreelancerPlatform.Application.Dtos.Category;
using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface ISkillService
    {
        Task<List<SkillQuickViewModel>> GetSkillAsync();
        Task<ServiceResult> CreateSkillAsync(SkillCreateRequest request);
        Task<ServiceResult> UpdateSkillAsync(SkillUpdateRequest request);
        Task<ServiceResult> DeleteSkillAsync(int id);
    }
}