using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("vai_tro_quan_tri")]
    public class Role : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ma_vai_tro")]
        public int Id { get; set; }

        [Required]
        [Column("ten_vai_tro")]
        public string Name { get; set; }

        public List<RolePermission> RolePermissions { get; set; }
        public List<SystemManagementRole> SystemManagementRoles { get; set; }
    }
}
