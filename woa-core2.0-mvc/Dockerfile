﻿FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# build runtime image
FROM microsoft/aspnetcore:2.0
# Set ASP.NET Core environment variables
#ENV ASPNETCORE_URLS="http://*:5000"
ENV ASPNETCORE_ENVIRONMENT="Docker"
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "woa-core2.0-mvc.dll"]