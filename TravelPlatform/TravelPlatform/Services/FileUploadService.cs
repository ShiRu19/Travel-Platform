using System.Threading;
using TravelPlatform.Handler;

namespace TravelPlatform.Services
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string fileHeader);
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly IFileUploadHandler _fileUploadHandler;

        public FileUploadService(IFileUploadHandler fileUploadHandler)
        {
            _fileUploadHandler = fileUploadHandler;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileHeader)
        {
            try
            {
                string fileUrl = await _fileUploadHandler.UploadFileAsync(file, fileHeader);
                return fileUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
