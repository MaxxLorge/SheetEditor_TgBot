using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4;
using LightInject;
using Microsoft.Extensions.Configuration;
using SheetEditor.Google;
using Telegram.Bot;

namespace SheetEditor.Extensions;

public static class LightInjectExtensions
{
    public static void RegisterDependencies(this ServiceContainer container)
    {
        container.RegisterAssembly(typeof(LightInjectExtensions).Assembly,
            lifetimeFactory: () => new PerScopeLifetime());

        container.Register<IConfigurationRoot>(_ => new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("secrets.json")
            .Build(), new PerContainerLifetime());
        
        container.Register<SheetsService>(_ =>
            GoogleServiceBuilder.Build<SheetsService>(AppDomain.CurrentDomain.FriendlyName, "google-secrets.json"));
        container.Register<DriveService>(_ =>
            GoogleServiceBuilder.Build<DriveService>(AppDomain.CurrentDomain.FriendlyName, "google-secrets.json"));
        
        container.Register<ITelegramBotClient>(factory =>
            {
                var apiKey = factory.GetInstance<IConfigurationRoot>()["TelegramApiKey"];
                return new TelegramBotClient(apiKey!);
            },
            new PerContainerLifetime());
        
        container.Register<ISheetHelper, SheetHelper>(new PerContainerLifetime());
    }
}