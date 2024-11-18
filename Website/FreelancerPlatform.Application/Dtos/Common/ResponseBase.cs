using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Common
{
    public class ResponseBase
    {
        public StatusResult Status { get; set; }
        public string Message { get; set; }
    }
}
