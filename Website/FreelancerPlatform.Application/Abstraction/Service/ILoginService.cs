using FreelancerPlatform.Application.Dtos.Common;
using FreelancerPlatform.Application.Dtos.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Abstraction.Service
{
    public interface ILoginService
    {
        Task<LoginResult> LoginAsync(LoginRequest request);
    }
}
