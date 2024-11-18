namespace FreelancerPlatform.Client.Models
{
    public class PostUpdateRequestForm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}
