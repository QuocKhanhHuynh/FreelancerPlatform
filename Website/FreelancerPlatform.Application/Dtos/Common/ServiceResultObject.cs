using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Common
{
    public class ServiceResultObject<T> : ServiceResult
    {
        public T Object { get; set; }
    }
}
