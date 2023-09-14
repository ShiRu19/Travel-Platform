using TravelPlatform.Models;
using TravelPlatform.Models.BackstageTravel;
using TravelPlatform.Models.Domain;
using TravelPlatform.Services.File.FileUpload;

namespace TravelPlatform.Services.TravelService.Backstage
{
    public class BackstageTravelEditService : IBackstageTravelEditService
    {
        private readonly TravelContext _db;
        private readonly IConfiguration _configuration;
        private readonly IFileUploadService _fileUploadService;

        public BackstageTravelEditService(TravelContext db, IConfiguration configuration, IFileUploadService fileUploadService)
        {
            _db = db;
            _configuration = configuration;
            _fileUploadService = fileUploadService;
        }

        /// <summary>
        /// 編輯場次
        /// </summary>
        /// <param name="sessionEditModel"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ResponseDto> EditSessionAsync(SessionEditModel sessionEditModel)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto { StatusCode = 400, Message = "", Error = "" };
            
            var sessionId = sessionEditModel.SessionId;
            var updSession = sessionEditModel.TravelSession;

            using (var transaction = _db.Database.BeginTransaction())
            {
                TravelSession session = new TravelSession();

                try
                {
                    var session_query = _db.TravelSessions.Where(t => t.Id == sessionId).FirstOrDefault();

                    if (session_query == null)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + "The travel does not exist");

                        response400.StatusCode = 404;
                        response400.Error = _configuration["ErrorMessage:NOT_FOUND"];
                        response400.Message = "The session does not exist.";
                        return response400;
                    }

                    session = session_query;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }

                session.ProductNumber = updSession.ProductNumber;
                session.DepartureDate = updSession.DepartureDate;
                session.Price = updSession.Price;
                session.RemainingSeats = updSession.Seats - updSession.Applicants;
                session.Seats = updSession.Seats;
                session.GroupStatus = updSession.GroupStatus;

                try
                {
                    _db.SaveChanges();
                    transaction.Commit();
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

        /// <summary>
        /// 編輯行程
        /// </summary>
        /// <param name="travelEditModel"></param>
        /// <returns></returns>
        public async Task<ResponseDto> EditTravelAsync(TravelEditModel travelEditModel)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto { StatusCode = 400, Message = "", Error = "" };

            long id = travelEditModel.Id;
            TravelInfoModel travelInfo = travelEditModel.TravelInfo;

            using (var transaction = _db.Database.BeginTransaction())
            {
                // Travel Information
                Models.Domain.Travel travel = new Models.Domain.Travel();
                try
                {
                    var travel_query = _db.Travels.Where(t => t.Id == id).SingleOrDefault();

                    if (travel_query == null)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + "The travel does not exist");

                        response400.StatusCode = 404;
                        response400.Error = _configuration["ErrorMessage:NOT_FOUND"];
                        response400.Message = "The travel does not exist.";
                        return response400;
                    }

                    travel = travel_query;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }

                travel.Title = travelInfo.Title;
                travel.DateRangeStart = travelInfo.DateRangeStart;
                travel.DateRangeEnd = travelInfo.DateRangeEnd;
                travel.Days = travelInfo.Days;
                travel.DepartureLocation = travelInfo.DepartureLocation;
                travel.Nation = travelInfo.Nation;

                // Main image
                if (travelInfo.MainImageFile != null)
                {
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
                        travel.MainImageUrl = mainImageUrl;
                    }
                    else
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + "Image file upload failed.");

                        response500.Error = _configuration["ErrorMessage:INTERNAL_SERVER_ERROR"];
                        response500.Message = "Image file upload failed.";
                        return response500;
                    }
                }

                // PDF
                if (travelInfo.PdfFile != null)
                {
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
                        travel.PdfUrl = pdfUrl;
                    }
                    else
                    {
                        transaction.Rollback();
                        Console.WriteLine("Transaction rolled back due to an error: " + "PDF file upload failed.");

                        response500.Error = _configuration["ErrorMessage:INTERNAL_SERVER_ERROR"];
                        response500.Message = "PDF file upload failed.";
                        return response500;
                    }
                }

                // Travel Attraction
                try
                {
                    var attractions = _db.TravelAttractions.Where(t => t.TravelId == id).ToList();
                    foreach (var attraction in attractions)
                    {
                        _db.TravelAttractions.Remove(attraction);
                    }

                    long travelAttractionId = _db.TravelAttractions.Count() == 0 ? 1 : _db.TravelAttractions.Max(t => t.Id) + 1;

                    foreach (string attraction in travelEditModel.TravelAttraction)
                    {
                        TravelAttraction newTravelAttraction = new TravelAttraction()
                        {
                            Id = travelAttractionId,
                            TravelId = id,
                            Attraction = attraction
                        };
                        _db.TravelAttractions.Add(newTravelAttraction);
                        travelAttractionId++;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);

                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }

                try
                {
                    _db.SaveChanges();
                    transaction.Commit();
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
