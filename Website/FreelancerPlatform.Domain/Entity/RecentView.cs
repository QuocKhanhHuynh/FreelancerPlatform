using FreelancerPlatform.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("cong_viec_xem_gan_day")]
    public class RecentView : EntityBase
    {
        public int JobId { get; set; }
        public Job Job { get; set; }
        public int FreelancerId { get; set; }
        public Freelancer Freelancer { get; set; }
    }
}
