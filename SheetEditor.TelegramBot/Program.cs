using LightInject;
using Microsoft.EntityFrameworkCore;
using SheetEditor.Data;
using SheetEditor.Extensions;
using Telegram.Bot;
using Telegram.Bot.Polling;

var container = new ServiceContainer();
container.RegisterDependencies();

var botClient = container.GetInstance<ITelegramBotClient>();

using var scope = container.BeginScope();
var context = scope.GetInstance<SheetEditorContext>();
context.Database.Migrate();
botClient.StartReceiving(scope.GetInstance<IUpdateHandler>());
Console.WriteLine("Бот начал принимать обновления");

while (true)
    Thread.Sleep(10000);