## Overview

Microsoft has deprecated Visual Studio for Mac so there is an entirely new way that projects need to be created for Visual Studio Code.  It is (well documented here)[https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio-code].  This is all run from the DynamicInstrumentation folder.  It will build a project folder and a .sln file in the same folder as it is run.

### Creating the solution and project
To create the solution and the associated project use the following:
```
dotnet new webapi -o DynamicInstrumentation
```

### Add a new Controller to the project
Add a controller needs a whole new set of tools (documented here)[https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio-code].  Use the following steps (from the project DynamicInstrumentation folder):

```
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet tool uninstall -g dotnet-aspnet-codegenerator
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet tool update -g dotnet-aspnet-codegenerator
dotnet aspnet-codegenerator controller -name DatadogOrganizationsController -outDir Controllers
```

The controller that got added still needed to be updated to look like the WeatherForecast controller, so further research needs to be done.