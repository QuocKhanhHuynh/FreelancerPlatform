using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Job
{
    public class FavoriteJobAddRequest
    {
        public int FreelancerId { get; set; }
        public int JobId { get; set; }
    }
}
