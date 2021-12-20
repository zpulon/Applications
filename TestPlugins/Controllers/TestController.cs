using ApiCore.Basic;
using ApiCore.Filters;
using ApiCore.JsonFilter;
using AspNet.Security.OAuth.Validation;
using AutoMapper;
using LogCore.Log;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TestPlugins.Dto.Response;
using TestPlugins.Managers;

namespace TestPlugins.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Route("api/test")]
    public class TestController : BaseController
    {
        private readonly UserManager _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger logger = LoggerManager.GetLogger("test");
        private readonly IJsonHelper _jsonHelper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="mapper"></param>
        /// <param name="jsonHelper"></param>
        public TestController(UserManager userManager, IMapper mapper, IJsonHelper jsonHelper) 
        {
            _userManager = userManager;
            _mapper = mapper;
            _jsonHelper = jsonHelper;
        }
        /// <summary>
        /// TOKEN
        /// </summary>
        /// <returns></returns>
        [HttpGet("info")]
        [AuthorizationLocal]
        public ActionResult<UserInfo> GetUserInfo()
        {
            try
            {
                return User;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("byid")]
        [AuthorizationLocal]
        public async Task<ResponseMessage<UserResponse>> GetUserInfoById([FromQuery]long id)
        {
            try
            {
                ResponseMessage<UserResponse> response = new ResponseMessage<UserResponse>();
                var user=await _userManager.Get_UserByIdAsync(id);
                response.Extension = _mapper.Map<UserResponse>(user);
                return response;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("list")]
        [AuthorizationLocal]
        public async Task<PagingResponseMessage<UserResponse>> GetUserList()
        {
            try
            {
                var result= await _userManager.Get_UserListAsync();
                logger.Info($"GetUserList:\r\n {_jsonHelper.ToJson(result)}");
                return result;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
