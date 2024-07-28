using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Formatting.Json;

namespace DynamicInstrumentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatadogOrganizationsController : ControllerBase
    {
        [HttpGet(Name = "GetDatadogOrganizations")]
        public string Index()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(new JsonFormatter())
                .CreateLogger();
            
            Log.Information("Calling HttpHandler");
            var httpHandler = new HttpHandler();
            var organizationInfo = httpHandler.GetServiceDefinitions();
            Log.Information(organizationInfo);
            return organizationInfo;
        }
    }
}