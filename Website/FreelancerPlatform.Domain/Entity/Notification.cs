using FreelancerPlatform.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("thong_bao")]
    public class Notification : EntityBase
    {
        [Key]
        [Column("ma_thong_bao")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("noi_dung_thong_bao")]
        [Required]
        public string Content { get; set; }

        [Column("trang_thai_da_xem")]
        public bool SeenStatus { get; set; } = false;

        public int FreelancerId { get; set; }
        public Freelancer Freelancer { get; set; }
    }
}
