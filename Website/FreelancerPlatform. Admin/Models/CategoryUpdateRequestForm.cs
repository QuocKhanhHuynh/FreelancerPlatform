namespace FreelancerPlatform._Admin.Models
{
    public class CategoryUpdateRequestForm
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}
