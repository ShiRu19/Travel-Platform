namespace TravelPlatform.Services;
public interface IFileUploadService
{
    Task<string> UploadFileAsync(IFormFile file, string fileHeader);
    Task<bool> ConfirmExtensionAsync(IFormFile file, string fileType);
}
