#установка окружения
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
80 #открытие порта 80
EXPOSE 
#настройка директорий
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
#не забудьте заменить на свои значения!
COPY ["./WebApplication3/WebApplication3.csproj", "WebApplication3/"] 
RUN dotnet restore "WebApplication3/WebApplication3.csproj"
COPY . .
WORKDIR "/src/WebApplication3"
RUN dotnet build "WebApplication3.csproj" -c Release -o /app/build
#упаковка файлов
FROM build AS publish
RUN dotnet publish "WebApplication3.csproj" -c Release -o /app/publish /p:UseAppHost=false
#запуск приложения
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplication3.dll"]
