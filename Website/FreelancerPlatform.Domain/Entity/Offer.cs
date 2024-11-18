using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    public class Offer : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ma_loi_moi")]
        public int Id { get; set; }

        public Freelancer Freelancer { get; set; }
        [Required]
        [Column("ma_ung_vien")]
        public int FreelancerId { get; set; }

        [Required]
        [Column("ma_nha_tuyen_dung")]
        public int FreelancerOfferId { get; set; } 

        public Job Job { get; set; }
        public int JobId { get; set; }
    }
}
