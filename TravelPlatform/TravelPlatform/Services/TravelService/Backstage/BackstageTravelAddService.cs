using TravelPlatform.Models;
using TravelPlatform.Models.BackstageTravel;
using TravelPlatform.Models.Domain;
using TravelPlatform.Services.File.FileUpload;

namespace TravelPlatform.Services.TravelService.Backstage
{
    public class BackstageTravelAddService : IBackstageTravelAddService
    {
        private readonly TravelContext _db;
        private readonly IConfiguration _configuration;
        private readonly IFileUploadService _fileUploadService;

        public BackstageTravelAddService(TravelContext db, IConfiguration configuration, IFileUploadService fileUploadService)
        {
            _db = db;
            _configuration = configuration;
            _fileUploadService = fileUploadService;
        }

        /// <summary>
        /// 新增場次
        /// </summary>
        /// <param name="sessionAddModel"></param>
        /// <returns></returns>
        public async Task<ResponseDto> AddSessionAsync(SessionAddModel sessionAddModel)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };

            var travelId = sessionAddModel.TravelId;

            using (var transaction = _db.Database.BeginTransaction())
            {
                long sessionId = 0;

                try
                {
                    sessionId = _db.TravelSessions.Max(s => s.Id) == 0 ? 1 : _db.TravelSessions.Max(s => s.Id) + 1;
                }
                catch (Exception ex)
                {
                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }

                foreach (TravelSessionModel travelSession in sessionAddModel.TravelSession)
                {
                    TravelSession newTravelSession = new TravelSession()
                    {
                        Id = sessionId,
                        TravelId = travelId,
                        ProductNumber = travelSession.ProductNumber,
                        DepartureDate = travelSession.DepartureDate,
                        Price = travelSession.Price,
                        RemainingSeats = travelSession.Seats - travelSession.Applicants,
                        Seats = travelSession.Seats,
                        GroupStatus = travelSession.GroupStatus
                    };
                    _db.TravelSessions.Add(newTravelSession);
                    sessionId++;
                }

                try
                {
                    _db.SaveChanges();
                    transaction.Commit();
                    Console.WriteLine("Transaction committed successfully.");

                    return response200;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }
            }
        }

        /// <summary>
        /// 新增行程
        /// </summary>
        /// <param name="travelAddModel"></param>
        /// <returns></returns>
        public async Task<ResponseDto> AddTravelAsync(TravelAddModel travelAddModel)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto { StatusCode = 400, Message = "", Error = "" };

            TravelInfoModel travelInfo = travelAddModel.TravelInfo;

            using (var transaction = _db.Database.BeginTransaction())
            {
                long travelId = 0;
                long travelAttractionId = 0;
                long travelSessionId = 0;

                try
                {
                    travelId = _db.Travels.Count() == 0 ? 1 : _db.Travels.Max(t => t.Id) + 1;
                    travelAttractionId = _db.TravelAttractions.Count() == 0 ? 1 : _db.TravelAttractions.Max(t => t.Id) + 1;
                    travelSessionId = _db.TravelSessions.Count() == 0 ? 1 : _db.TravelSessions.Max(t => t.Id) + 1;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }

                Models.Domain.Travel newTravel = new Models.Domain.Travel()
                {
                    Id = travelId,
                    Title = travelInfo.Title,
                    DateRangeStart = travelInfo.DateRangeStart,
                    DateRangeEnd = travelInfo.DateRangeEnd,
                    Days = travelInfo.Days,
                    DepartureLocation = travelInfo.DepartureLocation,
                    Nation = travelInfo.Nation
                };

                // Main image
                var isImageLegal = await _fileUploadService.ConfirmExtensionAsync(travelInfo.MainImageFile, "image");

                if (isImageLegal == false)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + "It's not image file.");

                    response400.Error = _configuration["ErrorMessage:BAD_REQUEST"];
                    response400.Message = "It's not image file.";
                    return response400;
                }

                var mainImageUrl = await _fileUploadService.UploadFileAsync(travelInfo.MainImageFile, "image");

                if (mainImageUrl != null)
                {
                    newTravel.MainImageUrl = mainImageUrl;
                }
                else
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + "Image file upload failed.");

                    response500.Error = _configuration["ErrorMessage:INTERNAL_SERVER_ERROR"];
                    response500.Message = "Image file upload failed.";
                    return response500;
                }

                // PDF
                var isPdfLegal = await _fileUploadService.ConfirmExtensionAsync(travelInfo.MainImageFile, "image");

                if (isPdfLegal == false)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + "It's not pdf file.");

                    response400.Error = _configuration["ErrorMessage:BAD_REQUEST"];
                    response400.Message = "It's not pdf file.";
                    return response400;
                }

                var pdfUrl = await _fileUploadService.UploadFileAsync(travelInfo.PdfFile, "pdf");

                if (pdfUrl != null)
                {
                    newTravel.PdfUrl = pdfUrl;
                }
                else
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + "PDF file upload failed.");

                    response500.Error = _configuration["ErrorMessage:INTERNAL_SERVER_ERROR"];
                    response500.Message = "PDF file upload failed.";
                    return response500;
                }

                _db.Travels.Add(newTravel);

                foreach (string attraction in travelAddModel.TravelAttraction)
                {
                    TravelAttraction newTravelAttraction = new TravelAttraction()
                    {
                        Id = travelAttractionId,
                        TravelId = travelId,
                        Attraction = attraction
                    };
                    _db.TravelAttractions.Add(newTravelAttraction);
                    travelAttractionId++;
                }

                foreach (TravelSessionModel travelSession in travelAddModel.TravelSession)
                {
                    TravelSession newTravelSession = new TravelSession()
                    {
                        Id = travelSessionId,
                        TravelId = travelId,
                        ProductNumber = travelSession.ProductNumber,
                        DepartureDate = travelSession.DepartureDate,
                        Price = travelSession.Price,
                        RemainingSeats = travelSession.Seats - travelSession.Applicants,
                        Seats = travelSession.Seats,
                        GroupStatus = travelSession.GroupStatus
                    };
                    _db.TravelSessions.Add(newTravelSession);
                    travelSessionId++;
                }

                try
                {
                    _db.SaveChanges();
                    transaction.Commit();
                    Console.WriteLine("Transaction committed successfully.");

                    return response200;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_SAVE_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }
            }
        }
    }
}
