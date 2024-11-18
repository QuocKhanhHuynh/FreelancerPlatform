using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("linh_vuc_hoat_dong")]
    public class Category : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("linh_vuc_hoat_dong")]
        public int Id { get; set; }

        [Required]
        [Column("ten_linh_vuc")]
        public string Name { get; set; }


        [Required]
        [Column("duong_dan")]
        public string ImageUrl { get; set; }

        public List<FreelancerCategory> FreelancerCategories { get; set; }
		public List<Job> Jobs { get; set; }

    }
}
