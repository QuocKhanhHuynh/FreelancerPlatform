using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Post
{
    public class PostViewModel
    {
        public int Id { get; set; } 
        public string Title { get; set; }   
        public string Content { get; set; }
        public string ImageUrl  { get; set; }
        public DateTime CreateDay { get; set; }
        public string  FreelancerImageUrl { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }  
        public int FreelancerId { get; set; }
        public int LikeNumber { get; set; }
        public int CommentNumber { get; set; }
    }
}
