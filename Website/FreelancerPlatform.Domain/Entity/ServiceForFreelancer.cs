using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("dich_vu_cho-ung_vien")]
    public class ServiceForFreelancer : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ma_dich_vu_cho_ung_vien")]
        public int Id { get; set; }

        [Required]
        [Column("ten_dich_vu")]
        public string Name { get; set; }

        [Required]
        [Column("phi_dich_vu")]
        public int Fee { get; set; }

        public List<RequirementServiceByFreelancer> RequirementServiceByFreelancers { get; set; }
    }
}
