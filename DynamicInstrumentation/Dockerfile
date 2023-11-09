# Becuase the version needs to be updated with every build, need to add and ARG value that is overridden in the Jenkins
# docker build statement.
ARG DD_GIT_REPOSITORY_URL
ARG DD_GIT_COMMIT_SHA
ARG DD_VERSION
# Base image is the dotnet SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
# Sets the working directory inside of the container.  If the folder doesn't exist it get created.
WORKDIR /App

# Copies everything from the current folder (the project folder in this case to the ./App folder of the container)
COPY . ./

# Because we don't keep the NuGet files with the project, this will go out and redownload them if required.
RUN dotnet restore
# Compiles the app
RUN dotnet publish -c Release -o out

# Not exactly sure why this image isn't used from the start, but this adds the ASP.NET layer to the container.
FROM mcr.microsoft.com/dotnet/aspnet:7.0
# Need set the working directory on this image to /App as well.
WORKDIR /App

# This actually takes the binaries that were in the /App/out folder from the build and puts them in the App folder of this image.
COPY --from=build-env /App/out .

# Set the environment variables.  The version number is manipulated by the Jenkins job to use the approprivate version.
# See the Jenkins file to see where these variables are passed in.
ENV DD_SERVICE=dyanmicinstrumentation
ENV DD_ENV=prod
ENV DD_VERSION=${DD_VERSION}
ENV DD_GIT_REPOSITORY_URL=${DD_GIT_REPOSITORY_URL} 
ENV DD_GIT_COMMIT_SHA=${DD_GIT_COMMIT_SHA}

# Because this is dynamically instrumented, we don't need to call the Datadog tracer (see RSSFeedPuller for that example)
CMD ["dotnet", "DynamicInstrumentation.dll"]
