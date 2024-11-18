using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Chat
{
    public class ChatCreateRequest
    {
        public int? HubChatId { get; set; }
        public int FreelancerId { get; set; }
        public string Content { get; set; }
        public int FreelancerA {  get; set; }
        public int FreelancerB { get; set; }
    }
}
