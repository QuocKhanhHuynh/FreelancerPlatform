using FreelancerPlatform.Domain.Entity.Base;
using FreelancerPlatform.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("ung_tuyen_viec")]
    public class Apply : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ma_ung_tuyen")]
        public int Id { get; set; }

        [Column("luong_de_xuat")]
        public int? Deal {  get; set; }

        [Required]
        [Column("so_ngay_hoan_thanh")]
        public int ExecutionTime { get; set; }

        [Column("gioi_thieu")]
        public string? Introduction { get; set; }

        [Column("duoc_moi")]
        [Required]
        public bool IsOffer { get; set; } = false;

		public Freelancer Freelancer { get; set; }
        public int FreelancerId { get; set; }

        public Job Job { get; set; }
        public int JobId { get; set; }
    }
}
