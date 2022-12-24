using Telegram.Bot;
using Telegram.Bot.Types;

namespace SheetEditor.Handlers.Abstractions;

public interface IHandler
{
    Task Process(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
}