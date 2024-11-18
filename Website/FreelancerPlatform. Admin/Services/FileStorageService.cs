namespace FreelancerPlatform._Admin.Services
{
    public class FileStorageService : IStorageService
    {
       // private readonly string _productImageFolder;
        private const string PRODUCT_IMAGE_FOLDER_NAME = "Image";
        private readonly IConfiguration _configuration;

        public FileStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{PRODUCT_IMAGE_FOLDER_NAME}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_configuration["ImageCloud"], fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_configuration["ImageCloud"], fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
