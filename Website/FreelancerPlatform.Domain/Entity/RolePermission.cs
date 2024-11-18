using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("quyen_vai_tro")]
    public class RolePermission : EntityBase
    {
        public Role Role { get; set; }
        public int RoleId { get; set; }

        public Permission Permission { get; set; }
        public int PermissionId { get; set; }
    }
}
