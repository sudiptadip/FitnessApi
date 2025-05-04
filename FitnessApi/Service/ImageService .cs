using FitnessApi.IService;

namespace FitnessApi.Service
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _imageFolder;

        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
            _imageFolder = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(_imageFolder))
                Directory.CreateDirectory(_imageFolder);
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_imageFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }

        public async Task<string> UpdateImageAsync(string oldImagePath, IFormFile newFile)
        {
            if (!string.IsNullOrWhiteSpace(oldImagePath))
            {
                //var oldFullPath = Path.Combine(_env.WebRootPath, oldImagePath.TrimStart('/'));
                //if (System.IO.File.Exists(oldFullPath))
                //    System.IO.File.Delete(oldFullPath);
            }

            return await SaveImageAsync(newFile);
        }

        public async Task<bool> DeleteImageAsync(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return false;
            }

            var fullPath = Path.Combine(_env.WebRootPath, imagePath.TrimStart('/'));

            if (System.IO.File.Exists(fullPath))
            {
               // System.IO.File.Delete(fullPath);
                return true;
            }

            return false;
        }
    }

}
