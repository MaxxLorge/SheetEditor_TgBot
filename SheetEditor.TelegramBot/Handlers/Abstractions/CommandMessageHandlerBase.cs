using Telegram.Bot;
using Telegram.Bot.Types;

namespace SheetEditor.Handlers.Abstractions;

public abstract class CommandMessageHandlerBase : ICommandMessageHandler
{
    public abstract string MessageKey { get; }

    protected Message Message { get; private set; }
    protected string MessageText => Message.Text;
    protected long ChatId => Message.Chat.Id;
    protected User TelegramUser => Message.From;

    protected string[] MessageWords => MessageText?.Split(' ', StringSplitOptions.TrimEntries)
                                       ?? Array.Empty<string>();

    public async Task Process(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Message = update.Message;
        await Process();
    }

    public abstract Task Process();
}