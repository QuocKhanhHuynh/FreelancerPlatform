using FreelancerPlatform.Application.Abstraction.Service;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform.Client.Controllers.Components
{
    public class SkillViewComponent : ViewComponent
    {
        private readonly ISkillService _skillService;
        public SkillViewComponent(ISkillService skillService)
        {
            _skillService = skillService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var skills = await _skillService.GetSkillAsync();
            return View("Default", skills);
        }
    }
}
