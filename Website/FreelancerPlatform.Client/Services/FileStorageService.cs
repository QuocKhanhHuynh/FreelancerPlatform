namespace FreelancerPlatform.Client.Services
{
    public class FileStorageService : IStorageService
    {
        private readonly string _productImageFolder;
        private const string PRODUCT_IMAGE_FOLDER_NAME = "Image";

        public FileStorageService(IWebHostEnvironment webHostEnvironment)
        {
            _productImageFolder = Path.Combine(webHostEnvironment.WebRootPath, PRODUCT_IMAGE_FOLDER_NAME);
        }

        public string GetFileUrl(string fileName)
        {
            return $"/{PRODUCT_IMAGE_FOLDER_NAME}/{fileName}";
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_productImageFolder, fileName);
            using var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var filePath = Path.Combine(_productImageFolder, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
