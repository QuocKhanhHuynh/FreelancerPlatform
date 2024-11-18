using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Account
{
    public class PriorityUpdateRequest
    {
        public int AccountId { get; set; }
        public int Priority { get; set; }
    }
}
