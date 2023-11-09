## Overview

### File Layout

\
-- DynamicInstrumentation.sln - ASP.NET solution file
-- BuildCurrentVersion.ps1 - Versioning tool, uses year (major), month / day (mmdd) and minutes passed in the day to create version numbers
-- Jenkinsfile - Build file for Jenkins. Currently running as a local instance on the Mac.
-- main.tf - Used to build the Dynamic Instrumentation service in the Datadog catalog.
\-- DynamicInstrumentation
    -- DynamicInstrumentation.csproj - C# ASP.NET project
    -- README.md - Information needed for the application code.

## Jenkins Build (use this if you want to just understand steps)

1. Github Checkout - Self explanatory
2. Get Current Version - Runs the Powershell script to build a new version
3. Get Git SHA Value - shell script to get ```git rev-parse HEAD```
4. Build Docker Container - Self explanatory
5. Push Docker Image to Docker Hub - Self explanatory
6. Create YAML Deployment File From Template - need to add the sha value and version tags to the deployment
7. Deploy Application to Kubernetes - Needs work.  Doesn't currently deal with previously deployed versions
8. Build / Update Datadog Service Catalog - Updates the Service Catalog in Datadog using ```main.tf```


## Publishing the Application

The service should always be deployed (should put this in the build pipeline).  