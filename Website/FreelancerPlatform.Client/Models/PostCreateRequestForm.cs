namespace FreelancerPlatform.Client.Models
{
    public class PostCreateRequestForm
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}
