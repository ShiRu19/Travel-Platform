using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using TravelPlatform.Models.AwsS3;

namespace TravelPlatform.Services.File.Storage;

public class StorageService : IStorageService
{
    public async Task<S3ResponseDto> UploadFileAsync(S3Object s3obj, AwsCredentials awsCredentials)
    {
        DotNetEnv.Env.Load();
        var AwsKey = Environment.GetEnvironmentVariable("ASPNETCORE_IAM__ACCESS_KEY");
        var SecretKey = Environment.GetEnvironmentVariable("ASPNETCORE_IAM__SECRET_KEY");

        // Adding AWS credentials
        var credentials = new BasicAWSCredentials(AwsKey, SecretKey);

        // Specify the region
        var config = new AmazonS3Config()
        {
            RegionEndpoint = Amazon.RegionEndpoint.APNortheast1
        };

        var response = new S3ResponseDto();

        try
        {
            // Create the upload request
            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = s3obj.InputStream,
                Key = s3obj.Name,
                BucketName = s3obj.BucketName,
                CannedACL = S3CannedACL.NoACL
            };

            // Created an S3 client
            using var client = new AmazonS3Client(credentials, config);

            // Upload utility to s3
            var transferUtility = new TransferUtility(client);

            // We are actually uploading the file to S3
            await transferUtility.UploadAsync(uploadRequest);

            response.StatusCode = 200;
            response.Message = $"{s3obj.Name} has been uploaded successfully";
        }
        catch (AmazonS3Exception ex)
        {
            response.StatusCode = (int)ex.StatusCode;
            response.Message = ex.Message;
        }
        catch (Exception ex)
        {
            response.StatusCode = 500;
            response.Message = ex.Message;
        }

        return response;
    }
}
