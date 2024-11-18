using FreelancerPlatform.Domain.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreelancerPlatform.Domain.Entity
{
    public class FavoriteJob : EntityBase
    {

        public Job Job { get; set; }
        public int JobId { get; set; }
        public Freelancer Freelancer { get; set; }
        public int FreelancerId { get; set; }
    }
}
