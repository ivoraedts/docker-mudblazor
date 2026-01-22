FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Listen on HTTP and HTTPS on all interfaces inside container
ENV ASPNETCORE_URLS=https://+:8081;http://+:8080
# Certificate configuration
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=MyPassword123

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MyApplication/MyApplication.csproj", "MyApplication/" ]
COPY ["MyApplication.Client/MyApplication.Client.csproj", "MyApplication.Client/" ]

RUN dotnet restore "MyApplication/MyApplication.csproj"
COPY . .
WORKDIR "/src/MyApplication"
RUN dotnet publish "MyApplication.csproj" -c release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT [ "dotnet", "MyApplication.dll" ]