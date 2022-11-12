FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/TillWhen.Api/TillWhen.Api.csproj", "TillWhen.Api/"]
COPY ["src/TillWhen.Application/TillWhen.Application.csproj", "TillWhen.Application/"]
COPY ["src/TillWhen.Domain/TillWhen.Domain.csproj", "TillWhen.Domain/"]
COPY ["src/TillWhen.Database.SqlServer/TillWhen.Database.SqlServer.csproj", "TillWhen.Database.SqlServer/"]
RUN dotnet restore "TillWhen.Api/TillWhen.Api.csproj"
COPY src/. .
WORKDIR "/src/TillWhen.Api"
RUN dotnet build "TillWhen.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TillWhen.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TillWhen.Api.dll"]
