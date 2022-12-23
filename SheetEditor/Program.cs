// See https://aka.ms/new-console-template for more information

using LightInject;
using Microsoft.Extensions.Configuration;
using SheetEditor;
using SheetEditor.Extensions;
using Telegram.Bot;

var container = new ServiceContainer();
container.RegisterDependencies();
container.Register<IConfigurationRoot>(_ => new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile("secrets.json")
    .Build(), new PerContainerLifetime());

var token = container.GetInstance<IConfigurationRoot>()["TelegramApiToken"];
if (token == null)
    throw new ArgumentNullException(nameof(token));

var botClient = new TelegramBotClient(token);
container.Register<ITelegramBotClient>(_ => botClient, new PerContainerLifetime());

botClient.StartReceiving<UpdateHandler>();
Console.WriteLine("Бот начал принимать обновления");

Console.ReadLine();
    