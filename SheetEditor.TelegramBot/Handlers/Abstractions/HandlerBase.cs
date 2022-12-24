using Microsoft.EntityFrameworkCore;
using SheetEditor.Data;
using SheetEditor.Data.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SheetEditor.Handlers.Abstractions;

public abstract class HandlerBase
{
    protected HandlerBase(SheetEditorContext context)
    {
        Context = context;
    }

    protected SheetEditorContext Context { get; }

    protected ITelegramBotClient BotClient { get; private set; } = null!;
    protected User TelegramUser { get; private set; }
    protected ApplicationUser ApplicationUser { get; private set; }

    protected async Task SetCommon(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        BotClient = botClient;
        TelegramUser = GetTelegramUser(update);
        var userFromDb = await Context
            .Users
            .Include(e => e.Spreadsheets)
            .FirstOrDefaultAsync(e => e.TelegramId == TelegramUser.Id, cancellationToken);

        if (userFromDb == null)
            userFromDb = Context.Users.Add(new ApplicationUser
            {
                TelegramId = TelegramUser.Id
            }).Entity;
        ApplicationUser = userFromDb;
        await Context.SaveChangesAsync(cancellationToken);
    }

    protected abstract User GetTelegramUser(Update update);
}