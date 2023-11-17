## Overview

Operational and Working - 11/17/2023

### ToDo

* Needs to return some kinds of datetime in the return JSON to differentiate runs
* Need to check on source code linking.  The service says it is set up, but UI isn't showing the code.

### Keys to Success

* Make sure the agent has the **remoteConfiguration** - **enabled: true** in the configuration (it is in helm-values.yaml in this repo)
* Make sure the environment variable **DD_DYNAMIC_INSTRUMENTATION_ENABLED** is enabled is set to true (in the dynamicinstrumentation-deployment-template.yaml file in this repo)
* Getting Source Code Linking is very helpful as it will show the code to be instrumented.  This is set in the dynamicinstrumentation-deployment-template.yaml file in this repo

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

1. Get Comparision Variables - Prevents redeployment of secrets and helm chart
2. Github Checkout - Self explanatory
3. Get Current Version - Runs the Powershell script to build a new version
4. Get Git SHA Value - shell script to get ```git rev-parse HEAD```
5. Update Docker Template - This doesn't really work, so it is down in Create YAML Deployment File
6. Build Docker Container - Self explanatory
7. Push Docker Image to Docker Hub - Self explanatory
8. Create YAML Deployment File From Template - need to add the sha value and version tags to the deployment
9. Install Kubernetes Secrets - There is a **when** statement to see if they are already installed.
10. Deploy Helm Chart - There is a **when** statement to see if they are already installed.
11. Deploy Application to Kubernetes - Needs work.  Doesn't currently deal with previously deployed versions
12. Build / Update Datadog Service Catalog - Updates the Service Catalog in Datadog using ```main.tf```


## Publishing the Application

The service should always be deployed (should put this in the build pipeline).  To expose this app use the following command:

```
minikube service dynamicinstrumentation-service --url
```