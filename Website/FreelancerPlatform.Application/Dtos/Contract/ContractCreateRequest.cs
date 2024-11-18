using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Contract
{
    public class ContractCreateRequest
    {
        public int CreateUser { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Partner { get; set; }
        public int JobId { get; set; }

    }
}
