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
    [Table("bai_dang")]
    public class Post : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ma_bai_dang")]
        public int Id { get; set; }

        [Required]
        [Column("ten_bai_dang")]
        public string Title { get; set; }

        [Required]
        [Column("noi_dung")]
        public string Content { get; set; }

        [Column("luot_thich")]
        public int LikeNumber { get; set; } = 0;

        [Column("trang_thai_duyet")]
        public bool ApproveStatus { get; set; }

        [Column("duong_dan_anh")]
        public string? ImageUrl { get; set; }
        public int FreelancerId { get; set; }
        public Freelancer Freelancer { get; set; }

        public List<Comment> Comments { get; set; }
        public List<SavePost> SavePosts { get; set; }
    }
}
