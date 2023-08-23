using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlatform.Models.BackstageTravel;
using TravelPlatform.Models.Domain;
using TravelPlatform.Services;

namespace TravelPlatform.Controllers.v1
{
    class TravelSessionList
    {
        public long Id { get; set; }
        public string ProductNumber { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string departureDate { get; set; } = null!;
        public int Days { get; set; }
        public int Price { get; set; }
        public int RemainingSeats { get; set; }
        public int Seats { get; set; }
        public int GroupStatus { get; set; }
    }

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BackstageTravelController : ControllerBase
    {
        private readonly TravelContext _db;
        private readonly IWebHostEnvironment _environment;
        private readonly IFileUploadService _fileUploadService;

        public BackstageTravelController(TravelContext db, IWebHostEnvironment environment, IFileUploadService fileUploadService)
        {
            _db = db;
            _environment = environment;
            _fileUploadService = fileUploadService;
        }

        /// <summary>
        /// Only get open travel list
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetOpenTravelList")]
        public IActionResult GetOpenTravelList()
        {
            try
            {
                var travels = _db.Travels.Where(t => t.DateRangeEnd >= DateTime.Now)
                    .Select(t => new
                    {
                        t.Id,
                        t.Title,
                        DateRange = t.DateRangeStart.ToString("d") + " ~ " + t.DateRangeEnd.ToString("d"),
                        t.Days,
                        t.Nation,
                        t.DepartureLocation
                    }).ToList();

                var result = new
                {
                    data = travels
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Only get close travel list
        /// </summary>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetCloseTravelList")]
        public IActionResult GetCloseTravelList()
        {
            try
            {
                var travels = _db.Travels.Where(t => t.DateRangeEnd < DateTime.Now)
                    .Select(t => new
                    {
                        t.Id,
                        t.Title,
                        DateRange = t.DateRangeStart.ToString("d") + " ~ " + t.DateRangeEnd.ToString("d"),
                        t.Days,
                        t.Nation,
                        t.DepartureLocation
                    }).ToList();

                var result = new
                {
                    data = travels
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Only get open travel session list
        /// </summary>
        /// <returns>Backstage Travel List Object</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetOpenTravelSessionList")]
        public IActionResult GetOpenTravelSessionList(long id)
        {
            try
            {
                var travelSession = _db.TravelSessions
                    .Where(s => s.TravelId == id && s.DepartureDate >= DateTime.Now)
                    .Select(s => new
                    {
                        s.Id,
                        s.ProductNumber,
                        s.DepartureDate,
                        s.Price,
                        s.RemainingSeats,
                        s.Seats,
                        s.GroupStatus
                    });

                var travel = _db.Travels.SingleOrDefault(t => t.Id == id);

                if(travel == null)
                {
                    return BadRequest();
                }

                var data = new List<TravelSessionList>();
                foreach(var session in travelSession)
                {
                    var sessionData = new TravelSessionList()
                    {
                        Id = session.Id,
                        ProductNumber = session.ProductNumber,
                        Title = travel.Title,
                        departureDate = session.DepartureDate.ToString("yyyy/MM/dd"),
                        Days = travel.Days,
                        Price = session.Price,
                        RemainingSeats = session.RemainingSeats,
                        Seats = session.Seats,
                        GroupStatus = session.GroupStatus
                    };

                    data.Add(sessionData);
                }

                var result = new
                {
                    data = data
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Only get close travel session list
        /// </summary>
        /// <returns>Backstage Travel List Object</returns>
        [MapToApiVersion("1.0")]
        [HttpGet("GetCloseTravelSessionList")]
        public IActionResult GetCloseTravelSessionList(long id)
        {
            try
            {
                var travelSession = _db.TravelSessions
                    .Where(s => s.TravelId == id && s.DepartureDate < DateTime.Now)
                    .Select(s => new
                    {
                        s.Id,
                        s.ProductNumber,
                        s.DepartureDate,
                        s.Price,
                        s.RemainingSeats,
                        s.Seats,
                        s.GroupStatus
                    });

                var travel = _db.Travels.SingleOrDefault(t => t.Id == id);

                if (travel == null)
                {
                    return BadRequest();
                }

                var data = new List<TravelSessionList>();
                foreach (var session in travelSession)
                {
                    var sessionData = new TravelSessionList()
                    {
                        Id = session.Id,
                        ProductNumber = session.ProductNumber,
                        Title = travel.Title,
                        departureDate = session.DepartureDate.ToString("yyyy/MM/dd"),
                        Days = travel.Days,
                        Price = session.Price,
                        RemainingSeats = session.RemainingSeats,
                        Seats = session.Seats,
                        GroupStatus = session.GroupStatus
                    };

                    data.Add(sessionData);
                }

                var result = new
                {
                    data = data
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Insert a new travel
        /// </summary>
        /// <param name="travelAddModel"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("AddTravel")]
        public async Task<IActionResult> AddTravel([FromForm] TravelAddModel travelAddModel)
        {
            TravelInfoModel travelInfo = travelAddModel.TravelInfo;
            
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    long travelId = _db.Travels.Count() == 0 ? 1 : _db.Travels.Max(t => t.Id) + 1;
                    long travelAttractionId = _db.TravelAttractions.Count() == 0 ? 1 : _db.TravelAttractions.Max(t => t.Id) + 1;
                    long travelSessionId = _db.TravelSessions.Count() == 0 ? 1 : _db.TravelSessions.Max(t => t.Id) + 1;

                    Travel newTravel = new Travel()
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
                    var mainImageUrl = await _fileUploadService.UploadFileAsync(travelInfo.MainImageFile, "mainImage");

                    if (mainImageUrl != null)
                    {
                        newTravel.MainImageUrl = mainImageUrl;
                    }
                    else
                    {
                        Console.WriteLine("[api/v1.0/TravelBackstage/AddTravel] Error: Main image file upload failed.");
                        return StatusCode(500);
                    }

                    // PDF
                    var pdfUrl = await _fileUploadService.UploadFileAsync(travelInfo.PdfFile, "pdf");

                    if (pdfUrl != null)
                    {
                        newTravel.PdfUrl = pdfUrl;
                    }
                    else
                    {
                        Console.WriteLine("[api/v1.0/TravelBackstage/AddTravel] Error: Pdf file upload failed.");
                        return StatusCode(500);
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

                    _db.SaveChanges();
                    transaction.Commit();
                    Console.WriteLine("Transaction committed successfully.");
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    Console.WriteLine("Transaction rolled back due to an error: " + ex.Message);
                    return StatusCode(500, ex.Message);
                }
            }
        }
    }
}
