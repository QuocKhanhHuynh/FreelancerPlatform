using FreelancerPlatform.Domain.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Domain.Entity
{
    public class LikePost : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int FreelancerId { get; set; }
        public Freelancer Freelancer { get; set; }
    }
}
