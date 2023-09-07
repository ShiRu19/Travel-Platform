using TravelPlatform.Models.AwsS3;

namespace TravelPlatform.Services;

public interface IStorageService
{
    Task<S3ResponseDto> UploadFileAsync(S3Object s3obj, AwsCredentials awsCredentials);
}
