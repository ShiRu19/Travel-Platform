﻿using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TravelPlatform.Models.AwsS3;
using TravelPlatform.Services;

namespace TravelPlatform.Handler
{
    public interface IFileUploadHandler
    {
        Task<string> UploadFileAsync(IFormFile file, string fileHeader);
    }

    public class FileUploadHandler : IFileUploadHandler
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;

        public FileUploadHandler(IWebHostEnvironment environment, IStorageService storageService, IConfiguration configuration)
        {
            _environment = environment;
            _storageService = storageService;
            _configuration = configuration;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileHeader)
        {
            string fileName = file.FileName;
            string ext = Path.GetExtension(fileName);
            string standardFileName = ConvertStandardFileFormat(fileHeader, ext);
            var uploadResult = await UploadFile(standardFileName, file, fileHeader);

            if(uploadResult.StatusCode != 200)
            {
                return null;
            }

            var url_s3 = _configuration["S3"];

            return $"{url_s3}{fileHeader}/{standardFileName}";
        }

        /// <summary>
        /// 圖檔名稱轉換
        /// </summary>
        /// <param name="extension">附檔名</param>
        /// <returns>新圖檔名稱</returns>
        private string ConvertStandardFileFormat(string fileHeader, string extension)
        {
            var now = DateTime.Now.ToString("yyyyMMddHHmmss");
            var result = $"{fileHeader}_{now}{extension}";
            return result;
        }

        private async Task<S3ResponseDto> UploadFile(string objName, IFormFile file, string fileHeader)
        {
            // Proccess the file
            await using var memoryStr = new MemoryStream();
            await file.CopyToAsync(memoryStr);

            string s3ObjectName = $"{fileHeader}/{objName}";

            var s3Obj = new Models.AwsS3.S3Object()
            {
                BucketName = "travelplatformbucket",
                InputStream = memoryStr,
                Name = s3ObjectName
            };

            DotNetEnv.Env.Load();

            var cred = new AwsCredentials()
            {
                AwsKey = Environment.GetEnvironmentVariable("ASPNETCORE_IAM__ACCESS_KEY"),
                AwsSecretKey = Environment.GetEnvironmentVariable("ASPNETCORE_IAM__SECRET_KEY")
            };

            var result = await _storageService.UploadFileAsync(s3Obj, cred);

            return result;
        }
    }
}
