using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Contract
{
    public class ContractQuickViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreateDay { get; set; }
        public int AcceptStatus { get; set; }
        public bool ContractStatus { get; set; }
        public int ProjectId {  get; set; } 
    }
}
