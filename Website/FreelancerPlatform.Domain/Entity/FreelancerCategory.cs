using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("ung_vien_linh_vuc")]
    public class FreelancerCategory : EntityBase
    {
        public Freelancer Freelancer { get; set; }
        public int FreelancerId { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
