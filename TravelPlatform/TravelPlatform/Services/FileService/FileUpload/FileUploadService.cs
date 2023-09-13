using System.Threading;
using TravelPlatform.Handler.File;

namespace TravelPlatform.Services.File.FileUpload
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IFileUploadHandler _fileUploadHandler;

        public FileUploadService(IFileUploadHandler fileUploadHandler)
        {
            _fileUploadHandler = fileUploadHandler;
        }

        public async Task<bool> ConfirmExtensionAsync(IFormFile file, string fileType)
        {
            try
            {
                bool result = await _fileUploadHandler.ConfirmExtensionAsync(file, fileType);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
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
