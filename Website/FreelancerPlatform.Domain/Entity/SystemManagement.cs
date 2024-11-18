using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("quan_tri_vien")]
    public class SystemManagement : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ma_quan_tri_vien")]
        public int Id { get; set; }

        [Required]
        [Column("ten_tai_khoan")]
        public string UserName { get; set; }

        [Required]
        [Column("mat_khau")]
        public string Password { get; set; }

        [Column("trang_thai_khoa")]
        public bool Status { get; set; } = false;

        [Column("so_dien_thoai")]
        public string? PhoneNumber { get; set; }
        /*public List<Freelancer> Freelancers { get; set; }
        public List<Job> Jobs { get; set; }
        public List<SystemManagementRole> SystemManagementRoles { get; set; }*/
    }
}
