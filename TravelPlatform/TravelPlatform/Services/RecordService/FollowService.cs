using Microsoft.AspNetCore.Mvc;
using TravelPlatform.Exceptions;
using TravelPlatform.Models;
using TravelPlatform.Models.Domain;
using TravelPlatform.Models.Record;

namespace TravelPlatform.Services.Record
{
    public class FollowService : IFollowService
    {
        private readonly TravelContext _db;
        private readonly IConfiguration _configuration;

        public FollowService(TravelContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<ResponseDto> AddFollowAsync(FollowModel followModel)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto();

            try
            {
                var follow = _db.Follows.Where(f => f.UserId == followModel.UserId && f.TravelId == followModel.TravelId).FirstOrDefault();
                
                if (follow != null)
                {
                    response400.StatusCode = 400;
                    response400.Error = _configuration["ErrorMessage:BAD_REQUEST"];
                    response400.Message = "Follow record already exists";
                    return response400;
                }
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            var addFollow = new Follow()
            {
                Id = _db.Follows.Count() == 0 ? 1 : _db.Follows.Max(f => f.Id) + 1,
                TravelId = followModel.TravelId,
                UserId = followModel.UserId
            };

            try
            {
                _db.Follows.Add(addFollow);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_SAVE_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            response200.Data = addFollow;
            return response200;
        }

        /// <summary>
        /// 取消追蹤
        /// </summary>
        /// <param name="followModel"></param>
        /// <returns></returns>
        public async Task<ResponseDto> CancelFollowAsync(FollowModel followModel)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };
            ResponseDto response400 = new ResponseDto();

            Follow follow = new Follow();

            try
            {
                var follow_query = _db.Follows.Where(f => f.UserId == followModel.UserId && f.TravelId == followModel.TravelId).FirstOrDefault();
                
                if (follow_query == null)
                {
                    response400.StatusCode = 400;
                    response400.Error = _configuration["ErrorMessage:BAD_REQUEST"];
                    response400.Message = "Follow record does not exist";
                    return response400;
                }

                follow = follow_query;
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            try
            {
                _db.Follows.Remove(follow);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_SAVE_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            return response200;
        }

        /// <summary>
        /// 確認使用者是否有追蹤此行程
        /// </summary>
        /// <param name="TravelId">行程id</param>
        /// <param name="UserId">使用者id</param>
        /// <returns></returns>
        public async Task<ResponseDto> CheckFollowAsync(FollowModel followModel)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };

            try
            {
                var follow = _db.Follows.Where(f => f.UserId == followModel.UserId && f.TravelId == followModel.TravelId).FirstOrDefault();

                if(follow == null)
                {
                    response200.Message = "User does not follow the travel";
                }
                else
                {
                    response200.Message = "User is following the travel";
                }

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
        /// 取得指定使用者的追蹤列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<ResponseDto> GetUserFollowListAsync(long UserId)
        {
            ResponseDto response200 = new ResponseDto { StatusCode = 200, Message = "", Data = new { } };
            ResponseDto response500 = new ResponseDto { StatusCode = 500, Message = "", Error = "" };

            List<Follow> follow_all = new List<Follow>();
            try
            {
                follow_all = _db.Follows.Where(f => f.UserId == UserId).ToList();
            }
            catch (Exception ex)
            {
                response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                response500.Message = ex.Message;
                return response500;
            }

            var followList_open = new List<Models.Domain.Travel>();
            var followList_close = new List<Models.Domain.Travel>();

            foreach (var follow in follow_all)
            {
                try
                {
                    var travel = _db.Travels.Where(t => t.Id == follow.TravelId).FirstOrDefault();
                    if(travel != null)
                    {
                        if (travel.DateRangeEnd < DateTime.UtcNow)
                        {
                            followList_close.Add(travel);
                        }
                        else
                        {
                            followList_open.Add(travel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    response500.Error = _configuration["ErrorMessage:DB_OP_EX"];
                    response500.Message = ex.Message;
                    return response500;
                }
            }

            response200.Data = new
            {
                open = followList_open,
                close = followList_close
            };

            return response200;
        }
    }
}
