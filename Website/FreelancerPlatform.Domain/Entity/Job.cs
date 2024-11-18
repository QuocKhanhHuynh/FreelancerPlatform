using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("cong_viec")]
    public class Job : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ma_cong_viec")]
        public int Id { get; set; }

        [Required]
        [Column("ten_viec")]
        public string Name { get; set; }


        [Column("mo_ta")]
		[Required]
		public string Description { get; set; }

        [Required]
        [Column("luong_dau")]
        public int MinDeal { get; set; }

        [Column("luong_cuoi")]
        public int? MaxDeal { get; set; }

        [Required]
        [Column("loai_luong")]
        public int SalaryType { get; set; }

        [Required]
        [Column("hinh_thuc_lam_viec")]
        public int WorkType { get; set; }

        [Required]
        [Column("loai_cong_viec")]
        public int JobType { get; set; }

        [Required]
        [Column("do_uu_tien")]
        public int Priority { get; set; } = 0;

        [Column("an_cong_viec")]
        public bool IsHiden { get; set; } = false;

        [Column("yeu_cau")]
        public string? Requirement { get; set; }

        [Column("so_ngay_du_kien_hoan_thanh")]
        public int? EstimatedCompletion {  get; set; }
        [Column("so_gio_lam_viec_ngay")]
        public int? HourPerDay { get; set; }



        //public List<Contract> Contracts { get; set; }
        public List<Apply> Applies { get; set; }
        public List<RecentView> RecentViews { get; set; }
        public List<FavoriteJob> FavoriteJobs { get; set; }
        public List<Offer> Offers { get; set; }
        public List<JobSkill> JobSkills { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public Freelancer Freelancer { get; set; }
        public int FreelancerId { get; set; }
    }
}
