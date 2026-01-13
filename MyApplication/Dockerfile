FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Listen on HTTP on all interfaces inside container so host -> container port mapping works.
ENV ASPNETCORE_URLS=http://+:8080
# Signal to the app that it's running in a container (used to skip HTTPS redirection).
ENV DOTNET_RUNNING_IN_CONTAINER=true

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