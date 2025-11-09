using Microsoft.AspNetCore.Hosting; // Para IWebHostEnvironment
using System;
using System.IO;
using System.Threading.Tasks;

#nullable enable

namespace Spendnt.API.Helpers
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalFileStorage(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFileAsync(string filePath, string containerName)
        {
            if (string.IsNullOrEmpty(filePath)) return Task.CompletedTask;

            var fileName = Path.GetFileName(filePath); 
            var directoryPath = Path.Combine(_env.WebRootPath, containerName);
            var fullPath = Path.Combine(directoryPath, fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }

        public async Task<string> SaveFileAsync(byte[] content, string extension, string containerName)
        {
            var fileName = $"{Guid.NewGuid()}{extension}";
            var folder = Path.Combine(_env.WebRootPath, containerName); 
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string savingPath = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(savingPath, content);

            // Construir la URL relativa para acceder al archivo
            var request = _httpContextAccessor.HttpContext?.Request
                          ?? throw new InvalidOperationException("There is no active HTTP context to build the file URL.");
            var url = $"{request.Scheme}://{request.Host}{request.PathBase}/{containerName}/{fileName}";
            return url;
        }
    }
}