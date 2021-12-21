using ApiCore.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SchoolServer.Controllers
{
    [Route("api/check")]
    public class CheckController : BaseController
    {
        [HttpHead]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return Content("OK");
        }
    }
}
