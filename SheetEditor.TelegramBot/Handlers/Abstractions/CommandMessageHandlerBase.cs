using Microsoft.EntityFrameworkCore;
using SheetEditor.Data;
using SheetEditor.Data.Entities;
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

    protected ITelegramBotClient BotClient { get; private set; } = null!;
    protected Message Message { get; private set; } = null!;
    protected string MessageText => Message.Text!;
    protected long ChatId => Message.Chat.Id;
    protected User TelegramUser => Message.From!;
    protected ApplicationUser ApplicationUser { get; private set; }

    protected string[] MessageWords => MessageText?.Split(' ', StringSplitOptions.TrimEntries)
                                       ?? Array.Empty<string>();

    public async Task Process(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Message = update.Message!;
        BotClient = botClient;
        var userFromDb = await Context
            .Users
            .FirstOrDefaultAsync(e => e.TelegramId == TelegramUser.Id, cancellationToken);
        
        if (userFromDb == null)
            userFromDb = Context.Users.Add(new ApplicationUser
            {
                TelegramId = TelegramUser.Id
            }).Entity;
        ApplicationUser = userFromDb;
        await Context.SaveChangesAsync(cancellationToken);
        await Handle(botClient, update, cancellationToken);
    }

    protected abstract Task Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

    protected async Task SendMessage(string text, ParseMode? parseMode = null,
        CancellationToken cancellationToken = default)
    {
        await BotClient.SendTextMessageAsync(ChatId,
            text,
            parseMode,
            cancellationToken: cancellationToken);
    }
}