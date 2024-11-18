using FreelancerPlatform.Application.Abstraction.Repository;
using FreelancerPlatform.Domain.Entity;
using FreelancerPlatform.Infratructure.Entityframework.Implementions.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Infratructure.Entityframework.Implementions.Repository
{
    public class RequirementServiceByFreelancerRepository : RepositoryBase<RequirementServiceByFreelancer>, IRequirementServiceByFreelancerRepository
    {
        public RequirementServiceByFreelancerRepository(ApplicationDbContext context) : base(context) { }
    }
}
