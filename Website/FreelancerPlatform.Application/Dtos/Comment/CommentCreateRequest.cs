using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Comment
{
    public class CommentCreateRequest
    {
        public int FreelancerId { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public int? Parent {  get; set; }
        public int? Reply { get; set; }
    }
}
