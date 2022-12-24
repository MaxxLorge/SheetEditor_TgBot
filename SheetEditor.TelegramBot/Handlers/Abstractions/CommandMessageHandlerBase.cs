using Microsoft.EntityFrameworkCore;
using SheetEditor.Data;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SheetEditor.Handlers.Abstractions;

public abstract class CommandMessageHandlerBase : ICommandMessageHandler
{
    protected SheetEditorContext Context { get; }

    public CommandMessageHandlerBase(SheetEditorContext context)
    {
        Context = context;
    }
    
    public abstract string MessageKey { get; }

    protected ITelegramBotClient BotClient { get; private set; }
    protected Message Message { get; private set; }
    protected string MessageText => Message.Text;
    protected long ChatId => Message.Chat.Id;
    protected User TelegramUser => Message.From;

    protected string[] MessageWords => MessageText?.Split(' ', StringSplitOptions.TrimEntries)
                                       ?? Array.Empty<string>();

    public async Task Process(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Message = update.Message;
        BotClient = botClient;
        var userFromDb = await Context
            .Users
            .FirstOrDefaultAsync(e => e.TelegramId == TelegramUser.Id, cancellationToken);

        if (userFromDb == null)
            Context.Users.Add(new Data.Entities.User
            {
                TelegramId = TelegramUser.Id
            });
        await Context.SaveChangesAsync(cancellationToken);
        await Handle(botClient, update, cancellationToken);
    }

    public abstract Task Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

    public async Task SendMessage(string text, ParseMode? parseMode = null,
        CancellationToken cancellationToken = default)
    {
        await BotClient.SendTextMessageAsync(ChatId,
            text,
            parseMode,
            cancellationToken: cancellationToken);
    }
}