using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreelancerPlatform.Application.Dtos.Post
{
    public class PostCreateRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public int FreelancerId { get; set; }
    }
}
