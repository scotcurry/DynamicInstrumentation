using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Formatting.Json;


namespace DynamicInstrumentation.Controllers;

[ApiController]
[Route("[controller]")]
public class ForceErrorController : ControllerBase
{
    
    public String Index()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();
        Log.Error("Getting Ready To Force An Error");
        List<int> intList = new List<int> { 0, 2 };
        var zero = 2 - intList[1];
        var errorVariable = 1 / zero;
        Log.Error(errorVariable.ToString());
        Log.Error("Error Should Have Happened");

        return "ForceErrorController";
    }
}