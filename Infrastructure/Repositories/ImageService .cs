using Application.Common.Options;
using Application.Dtos.ProductDto;
using Application.Interfaces.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class ImageService : IImageService
    {
        private readonly ProductImageSettings _settings;
        private readonly IWebHostEnvironment _env;

        public ImageService(
            IOptions<ProductImageSettings> settings,
            IWebHostEnvironment env)
        {
            _settings = settings.Value;
            _env = env;
        }

        public async Task<ProductImage> UploadProductImageAsync(
            Guid tenantId,
            Guid productId,
            UploadImageDto image,
            bool isMain)
        {
            ValidateFile(image);

            var folderPath = BuildFolderPath(tenantId, productId);
            Directory.CreateDirectory(folderPath);

            var fileName = GenerateFileName(image.FileName);
            var fullPath = Path.Combine(folderPath, fileName);

            try
            {
                await using var stream = new FileStream(
                    fullPath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None);

                await image.Content.CopyToAsync(stream);
            }
            catch
            {
                if (File.Exists(fullPath))
                    File.Delete(fullPath);

                throw;
            }

            return new ProductImage
            {
                ProductId = productId,
                ImageUrl = BuildRelativePath(tenantId, productId, fileName),
                IsMain = isMain
            };
        }

        public Task DeleteImageAsync(ProductImage image)
        {
            var fullPath = Path.Combine(_env.WebRootPath, image.ImageUrl.TrimStart('/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }

        // =========================
        // 🔹 Helpers
        // =========================

        private string BuildFolderPath(Guid tenantId, Guid productId)
        {
            var rootPath = _env.WebRootPath;

            if (string.IsNullOrEmpty(rootPath))
            {
                rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            return Path.Combine(
                rootPath,
                _settings.UploadFolder,
                tenantId.ToString(),
                productId.ToString());
        }

        private string BuildRelativePath(Guid tenantId, Guid productId, string fileName)
        {
            return "/" + Path.Combine(
                _settings.UploadFolder,
                tenantId.ToString(),
                productId.ToString(),
                fileName).Replace("\\", "/");
        }

        private string GenerateFileName(string originalFileName)
        {
            return $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
        }

        private void ValidateFile(UploadImageDto file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!_settings.AllowedExtensions.Contains(extension))
                throw new Exception("Invalid image extension");

            if (file.Length > _settings.MaxSizeInMB * 1024 * 1024)
                throw new Exception("Image size exceeded");
        }
    }
}