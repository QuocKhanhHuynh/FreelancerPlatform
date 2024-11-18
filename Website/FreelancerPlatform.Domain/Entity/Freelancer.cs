using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("ung_vien")]
    public class Freelancer : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        [Required]
		[Column("Email")]
		public string Email { get; set; }

        [Column("mat_khau")]
        public string? Password { get; set; }

        [Column("ho_ten_dem")]
        [Required]
        public string LastName { get; set; }
		[Column("ten")]
        [Required]
        public string FirstName { get; set; }

        [Column("duong_dan_anh")]
        public string? ImageUrl { get; set;}

        [Column("so_tai_khoan_ngan_hang")]
        public string? BankNumber { get; set;}

		[Column("ten_tai_khoan_ngan_hang")]
		public string? BankName { get; set; }

		[Column("gioi_thieu")]
        public string? About { get; set; }

        [Column("luong_mot_gio")]
        public int? RateHour { get; set; }

        [Column("xac_thuc_thong_tin_ca_nhan")]

		[Required]
		public bool PaymentVerification { get; set; } = false;

		[Required]
        [Column("do_uu_tien")]
        public int Priority { get; set; } = 0;

        [Column("trang_thai_khoa")]
        public bool Status { get; set; } = false;

        [Column("kinh_nghiem")]
        public string? Experience { get; set; }
        [Column("hoc_van")]
        public string? Education { get; set; }
        [Column("chung_chi")]
        public string? Certification { get; set; }
        [Column("thanh_tuu")]
        public string? Archive  { get; set; }


        public List<RequirementServiceByFreelancer> RequirementServiceByFreelancers { get; set; }

        public List<FreelancerCategory> FreelancerCategories { get; set; }

        public List<Apply> Applies { get; set; }
        public List<FavoriteJob> FavoriteJobs { get; set; }
        public List<PotentialFreelancer> PotentialFreelancers { get; set ; }
        public List<Offer> Offers { get; set; }

        public List<Report> Reports { get; set; }
        public List<FreelancerSkill> FreelancerSkills { get; set; }
        public List<Chat> Chats { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<Transaction> Transactions { get; set; }

        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }
        public List<SavePost> SavePosts { get; set; }
        public List<LikeComment> LikeComments { get; set; }
        public List<RecentView> RecentViews { get; set; }

    }
}
