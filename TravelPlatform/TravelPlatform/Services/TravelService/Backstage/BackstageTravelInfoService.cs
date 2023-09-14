using TravelPlatform.Handler.Response;
using TravelPlatform.Models;
using TravelPlatform.Models.BackstageTravel;
using TravelPlatform.Models.Domain;
using TravelPlatform.Services.Facebook;
using TravelPlatform.Services.File.FileUpload;
using TravelPlatform.Services.PasswordService;
using TravelPlatform.Services.Token;

namespace TravelPlatform.Services.TravelService.Backstage
{
    public class BackstageTravelInfoService : IBackstageTravelInfoService
    {
        private readonly TravelContext _db;
        private readonly IConfiguration _configuration;

        public BackstageTravelInfoService(TravelContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        /// <summary>
        /// 取得場次資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetSessionInfoAsync(long id)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto { StatusCode = 400, Message = "", Error = "" };

            try
            {
                var session = _db.TravelSessions.Where(t => t.Id == id).SingleOrDefault();
                
                if (session == null)
                {
                    response400.StatusCode = 404;
                    response400.Error = _configuration["ErrorMessage:NOT_FOUND"];
                    response400.Message = "The session does not exist.";
                    return response400;
                }

                response200.Data = new
                {
                    session.ProductNumber,
                    DepartureDate = session.DepartureDate.ToString("MM/dd/yyyy"),
                    session.Price,
                    applicants = session.Seats - session.RemainingSeats,
                    session.Seats,
                    session.GroupStatus
                };
                return response200;
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }
        }

        /// <summary>
        /// 取得行程資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetTravelInfoAsync(long id)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };

            try
            {
                var travel = _db.Travels.Where(t => t.Id == id)
                    .Select(t => new
                    {
                        t.Title,
                        t.DateRangeStart,
                        t.DateRangeEnd,
                        t.Days,
                        t.Nation,
                        departure_location = t.DepartureLocation,
                        pdf_url = t.PdfUrl,
                        main_image_url = t.MainImageUrl
                    });

                var attractions = _db.TravelAttractions.Where(t => t.TravelId == id)
                    .Select(t => t.Attraction).ToList();

                response200.Data = new
                {
                    travel,
                    attractions
                };
                return response200;
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }

        }
    }
}
