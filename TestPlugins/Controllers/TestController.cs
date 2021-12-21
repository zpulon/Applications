using ApiCore.Basic;
using ApiCore.Dto.Request;
using ApiCore.Filters;
using ApiCore.JsonFilter;
using ApiCore.Utils;
using AspNet.Security.OAuth.Validation;
using AutoMapper;
using LogCore.Log;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
        private readonly ILogger logger = LoggerManager.GetLogger("TestController");
        private readonly IJsonHelper _jsonHelper;
        private readonly IConfigurationRoot _config = null;
        private readonly HttpClientActuator _httpClient;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="mapper"></param>
        /// <param name="jsonHelper"></param>
        public TestController(UserManager userManager, IMapper mapper, IJsonHelper jsonHelper, IConfigurationRoot configuration, HttpClientActuator httpClientActuator) 
        {
            _userManager = userManager;
            _mapper = mapper;
            _jsonHelper = jsonHelper;
            _config = configuration;
            _httpClient = httpClientActuator;
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

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("send")]
        [AllowAnonymous]
        public async Task end()
        {
            try
            {
                string schedulerUrl = $"{_config["ScheduleServerUrl"]}/api/scheduler/start";
                var requestInfo = new ScheduleSubmitRequest
                {
                    JobGroup = 1.ToString(),
                    JobName = "tablename",
                    CronStr = "",
                    StarRunTime = DateTime.Now.AddSeconds(5),
                    EndRunTime = null,
                    Callback = $"{_config["AppGatewayUrl"]}/api/test/calback",
                    Args = new Dictionary<string, object> { {"key", 1 } },

                };
                logger.Info($"向任务调度中心添加，地址：{schedulerUrl}\r\n参数：{_jsonHelper.ToJson(requestInfo)}");
                await _httpClient.Post<ResponseMessage>(schedulerUrl, requestInfo);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// 回调
        /// </summary>
        /// <returns></returns>
        [HttpPost("calback")]
        [AllowAnonymous]
        public async Task Callback([FromBody] ScheduleExecuteRequest scheduleExecuteRequest)
        {
            try
            {
                var result = await _userManager.Get_UserListAsync();
                logger.Info($"GetUserList:\r\n {_jsonHelper.ToJson(scheduleExecuteRequest)}");
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
