using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("quyen")]
    public class Permission : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ma_quyen")]
        public int Id { get; set; }

        [Required]
        [Column("ten_quyen")]
        public string Name { get; set; }

        public List<RolePermission> RolePermissions { get; set; }
    }
}
