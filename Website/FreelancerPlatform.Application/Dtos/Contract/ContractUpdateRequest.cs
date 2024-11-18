using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Contract
{
    public class ContractUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}
