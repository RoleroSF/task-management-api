FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TaskManagementApi.csproj", "./"]
RUN dotnet restore "TaskManagementApi.csproj"

COPY . .
RUN dotnet build "TaskManagementApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TaskManagementApi.dll"]