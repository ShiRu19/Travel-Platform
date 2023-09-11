namespace TravelPlatform.Handler
{
    public interface IFileUploadHandler
    {
        Task<string> UploadFileAsync(IFormFile file, string fileHeader);
        Task<bool> ConfirmExtensionAsync(IFormFile file, string fileType);
    }
}
