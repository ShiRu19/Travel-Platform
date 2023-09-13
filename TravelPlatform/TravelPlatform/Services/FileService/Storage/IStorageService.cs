using TravelPlatform.Models.AwsS3;

namespace TravelPlatform.Services.File.Storage;

public interface IStorageService
{
    Task<S3ResponseDto> UploadFileAsync(S3Object s3obj, AwsCredentials awsCredentials);
}
