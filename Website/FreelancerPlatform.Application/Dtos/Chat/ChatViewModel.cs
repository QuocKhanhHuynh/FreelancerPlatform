using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Chat
{
    public class ChatViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateDay { get; set; }
        public int FreelancerId { get; set; }

        public string ImageUrl { get; set; }
    }
}
