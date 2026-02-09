FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src


COPY ["TaskManager.sln", "./"]
COPY ["TaskManager/TaskManager.csproj", "TaskManager/"]
COPY ["TaskManager.Worker/TaskManager.Worker.csproj", "TaskManager.Worker/"]


RUN dotnet restore "TaskManager.Worker/TaskManager.Worker.csproj"

COPY . .


WORKDIR "/src/TaskManager.Worker"
RUN dotnet build "TaskManager.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TaskManager.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManager.Worker.dll"]