namespace TravelPlatform.Handler
{
    public interface IFileUploadHandler
    {
        Task<string> UploadFileAsync(IFormFile file, string fileHeader);
    }

    public class FileUploadHandler : IFileUploadHandler
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadHandler(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string fileHeader)
        {
            string wwwPath = _environment.WebRootPath + "\\uploads\\";

            string fileName = file.FileName;
            string ext = Path.GetExtension(fileName);
            string standardFileName = ConvertStandardFileFormat(fileHeader, ext);

            using (var stream = File.Create(wwwPath + standardFileName))
            {
                await file.CopyToAsync(stream);
            }

            return $"uploads/{standardFileName}";
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
    }
}
