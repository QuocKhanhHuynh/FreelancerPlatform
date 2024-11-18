using FreelancerPlatform.Application.Abstraction.Service;
using Microsoft.AspNetCore.Mvc;

namespace FreelancerPlatform.Client.Controllers.Components
{
    public class SkillSearchJobViewComponent : ViewComponent
    {
        private readonly ISkillService _skillService;
        public SkillSearchJobViewComponent(ISkillService skillService)
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
