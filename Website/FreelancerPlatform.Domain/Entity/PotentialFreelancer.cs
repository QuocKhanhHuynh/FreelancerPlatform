using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("ung_vien_tim_nang")]
    public class PotentialFreelancer : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ma_ung_vien_tim_nang")]
        public int Id { get; set; }

        public Freelancer Freelancer { get; set; }
        public int FreelancerId { get; set; }
        public int FreelancerPotientId { get; set; }
    }
}
