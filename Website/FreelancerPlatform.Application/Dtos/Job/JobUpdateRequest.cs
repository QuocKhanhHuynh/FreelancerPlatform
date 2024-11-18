using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Job
{
    public class JobUpdateRequest : JobCreateRequest
    {
        public int Id { get; set; }
    }
}
