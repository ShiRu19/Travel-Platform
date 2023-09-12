namespace TravelPlatform.Models
{
    public class ResponseDto
    {
        public int StatusCode { get; set; }
        public string? Error { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
