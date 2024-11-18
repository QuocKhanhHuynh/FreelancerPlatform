using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.SystemManagement
{
    public class SystemManagementQuickViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreateDay { get; set; }   
        public bool Status { get; set; }
    }
}
