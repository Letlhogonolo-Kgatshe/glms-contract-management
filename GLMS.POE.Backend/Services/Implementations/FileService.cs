using Microsoft.AspNetCore.Components.Forms;
using GLMS.POE.Backend.Services.Interfaces;

namespace GLMS.POE.Backend.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileService> _logger;

        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf"
        };

        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

        public FileService(IWebHostEnvironment env, ILogger<FileService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public bool IsValidPdf(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return false;
            var ext = Path.GetExtension(fileName);
            return AllowedExtensions.Contains(ext);
        }

        public async Task<(string path, string fileName)> SaveContractFileAsync(IBrowserFile file)
        {
            if (!IsValidPdf(file.Name))
                throw new InvalidOperationException($"Only PDF files are allowed. Received: {Path.GetExtension(file.Name)}");

            if (file.Size > MaxFileSizeBytes)
                throw new InvalidOperationException($"File size exceeds the 10 MB limit.");

            var uniqueFileName = $"{Guid.NewGuid():N}_{Path.GetFileName(file.Name)}";

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "contracts");
            Directory.CreateDirectory(uploadsFolder);

            var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.OpenReadStream(MaxFileSizeBytes).CopyToAsync(stream);

            _logger.LogInformation("Saved contract file: {FileName} → {Path}", file.Name, fullPath);

            var relativePath = Path.Combine("uploads", "contracts", uniqueFileName);
            return (relativePath, file.Name);
        }

        public void DeleteFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return;
            var fullPath = Path.Combine(_env.WebRootPath, path);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation("Deleted file: {Path}", fullPath);
            }
        }
    }
}
