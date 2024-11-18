using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Domain.Entity
{
    [Table("chat_tap_trung")]
    public class HubChat
    {
        [Key]
        [Column("ma_chat_tap_trung")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int FreelancerA { get; set; }
        public int FreelancerB { get; set; }
        public bool SeenStatusA { get; set; } = false;
        public bool SeenStatusB { get; set; } = false;

        public List<Chat> Chats { get; set; }
    }
}
