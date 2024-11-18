using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Domain.Entity.Base
{
    public class EntityBase
    {
        [Required]
        [Column("ngay_tao")]
        public DateTime CreateDay { get; set; } = DateTime.Now;

        [Column("ngay_cap_nhat")]
        public DateTime? CreateUpdate { get; set; } 
    }
}
