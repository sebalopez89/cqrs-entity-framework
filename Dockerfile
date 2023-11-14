# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore
#RUN dotnet dev-certs https

# copy everything else and build app
WORKDIR /source/API
RUN dotnet publish -c release -o /api --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
LABEL author=”SebaLopez”

#ENV ASPNETCORE_URLS="https://*:5001;http://*:5000"
ENV ASPNETCORE_URLS="http://*:5000"
ENV ASPNETCORE_ENVIRONMENT="Development"

#EXPOSE    5000
#EXPOSE    1433

WORKDIR /api
COPY --from=build /api ./
ENTRYPOINT ["dotnet", "CQRS.Host.dll"]