using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int LikeNumber { get; set; }
        public int? Parent { get; set; }
        public int? Reply { get; set; }
        public int PostId { get; set; }
        public int FreelancerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string ImageUrl { get; set; }
        public int ReplyNumber { get; set; }
        public DateTime CreateDay { get; set; }
    }
}
