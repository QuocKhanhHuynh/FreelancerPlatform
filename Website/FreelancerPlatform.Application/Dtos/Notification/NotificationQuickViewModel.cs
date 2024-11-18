using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Notification
{
    public class NotificationQuickViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateDay { get; set; }
        public int FreelancerId { get; set; }
    }
}
