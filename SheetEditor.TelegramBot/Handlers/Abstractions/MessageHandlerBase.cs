using SheetEditor.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SheetEditor.Handlers.Abstractions;

public abstract class MessageHandlerBase : HandlerBase, IMessageHandler
{
    public MessageHandlerBase(SheetEditorContext context) : base(context)
    {
        
    }
    
    public abstract string MessageKey { get; }
    
    protected Message Message { get; private set; } = null!;
    protected string MessageText => Message.Text!;
    protected long ChatId => Message.Chat.Id;

    protected string[] MessageWords => MessageText?.Split(' ', StringSplitOptions.TrimEntries)
                                       ?? Array.Empty<string>();

    public async Task Process(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Message = update.Message!;
        await SetCommon(botClient, update, cancellationToken);
        await Handle(botClient, update, cancellationToken);
    }

    protected abstract Task Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

    protected async Task SendMessage(string text, ParseMode? parseMode = default,
        IReplyMarkup? replyMarkup = default,
        CancellationToken cancellationToken = default)
    {
        await BotClient.SendTextMessageAsync(ChatId,
            text,
            parseMode,
            replyMarkup: replyMarkup,
            cancellationToken: cancellationToken);
    }

    protected override User GetTelegramUser(Update update)
    {
        return update.Message.From;
    }
}