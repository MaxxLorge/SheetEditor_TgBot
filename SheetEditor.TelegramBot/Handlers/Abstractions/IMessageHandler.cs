using Telegram.Bot;
using Telegram.Bot.Types;

namespace SheetEditor.Handlers.Abstractions;

public interface IMessageHandler
{
    public string HelpDescription { get; }
    public string MessageKey { get; }
    Task Process(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
}