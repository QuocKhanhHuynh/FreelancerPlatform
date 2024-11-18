using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("dich_vu_ung_vien_yeu_cau")]
    public class RequirementServiceByFreelancer : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ma_ung_vien_yeu_cau")]
        public int Id { get; set; }

        [Required]
        [Column("trang_thai")]
        public int Status { get; set; }

        public ServiceForFreelancer ServiceForFreelancer { get; set; }
        public int ServiceForFreelancerId { get; set; }

        public Freelancer Freelancer { get; set; }
        public int FreelancerId { get; set;}
    }
}
