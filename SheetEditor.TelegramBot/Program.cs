using LightInject;
using SheetEditor.Extensions;
using Telegram.Bot;
using Telegram.Bot.Polling;

var container = new ServiceContainer();
container.RegisterDependencies();

var botClient = container.GetInstance<ITelegramBotClient>();

using var scope = container.BeginScope();
botClient.StartReceiving(scope.GetInstance<IUpdateHandler>());
Console.WriteLine("Бот начал принимать обновления");


Console.ReadLine();
    