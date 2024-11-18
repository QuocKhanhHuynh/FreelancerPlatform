using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("bao_cao")]
    public class Report : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ma_bao_cao")]
        public int Id { get; set; }

        [Required]
        [Column("noi_dung")]
        public string Content { get; set; }

        [Column("da_danh_gia")]
        public bool IsReview {  get; set; } = false;

        public Freelancer Freelancer { get; set; }
        public int FreelancerId { get; set; }

        public int UserReport { get; set; }
    }
}
