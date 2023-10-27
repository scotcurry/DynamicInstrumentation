using Microsoft.AspNetCore.Mvc;

namespace DynamicInstrumentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatadogOrganizationsController : ControllerBase
    {
        [HttpGet(Name = "GetDatadogOrganizations")]
        public string Index()
        {
            var httpHandler = new HttpHandler();
            var organizationInfo = httpHandler.GetJSONData();
            return organizationInfo;
        }
    }
}