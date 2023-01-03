FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SheetEditor.TelegramBot/SheetEditor.TelegramBot.csproj", "SheetEditor.TelegramBot/"]
COPY ["SheetEditor.Google/SheetEditor.Google.csproj", "SheetEditor.Google/"]
RUN dotnet restore "SheetEditor.TelegramBot/SheetEditor.TelegramBot.csproj"
COPY . .
WORKDIR "/src/SheetEditor.TelegramBot"
RUN dotnet build "SheetEditor.TelegramBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SheetEditor.TelegramBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "SheetEditor.TelegramBot.dll"]
