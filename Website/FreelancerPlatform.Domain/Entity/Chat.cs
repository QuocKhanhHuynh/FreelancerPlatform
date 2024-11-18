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
    [Table("tin_nhan")]
    public class Chat : EntityBase
    {
        [Key]
        [Column("ma_tin_nhan")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("noi_dung")]
        public string Content { get; set; }

        public int FreelancerId { get; set; }
        public Freelancer Freelancer { get; set; }

        
        public int HubChatId { get; set; }
        public HubChat HubChat { get; set; }
    }
}
