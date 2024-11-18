using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Notification
{
    public class CreateNotificationRequest
    {
        public string Content { get; set; }
        public int FreelancerId { get; set; }
    }
}
