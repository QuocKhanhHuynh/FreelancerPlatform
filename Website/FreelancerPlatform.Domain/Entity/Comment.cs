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
    [Table("binh_luan")]
    public class Comment : EntityBase
    {
        [Key]
        [Column("ma_binh_luan")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("noi_dung_binh_luan")]
        [Required]
        public string Content { get; set; }

        [Column("luot_thich")]
        public int LikeNumber { get; set; }
        public int? Parent {  get; set; }
        public int? Reply {  get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int FreelancerId { get; set; }
        public Freelancer Freelancer { get; set; }
        public List<LikeComment> Comments {  get; set; }
        public List<LikeComment> LikeComments { get; set; }

    }
}
