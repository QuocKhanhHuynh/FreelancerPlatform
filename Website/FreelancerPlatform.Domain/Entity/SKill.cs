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
	[Table("ky_nang")]
	public class SKill : EntityBase
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("ma_ky_nang")]
		public int Id { get; set; }

		[Required]
		[Column("ten_ky_nang")]
		public string Name { get; set; }
		public List<FreelancerSkill> FreelancerSkills { get; set; }
        public List<JobSkill> JobSkills { get; set; }
    }
}
