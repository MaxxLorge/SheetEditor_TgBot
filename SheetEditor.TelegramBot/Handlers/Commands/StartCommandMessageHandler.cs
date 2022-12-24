using Microsoft.EntityFrameworkCore;
using SheetEditor.Data;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = SheetEditor.Data.Entities.User;

namespace SheetEditor.Handlers.Commands;

public class StartCommandMessageHandler : ICommandMessageHandler
{
    private readonly SheetEditorContext _sheetEditorContext;

    public StartCommandMessageHandler(SheetEditorContext sheetEditorContext)
    {
        _sheetEditorContext = sheetEditorContext;
    }

    public string MessageKey => "/start";

    public async Task Process(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var user = update.Message?.From;
        if (user == null)
            throw new InvalidOperationException($"{nameof(user)} is null");

        var userFromDb = await _sheetEditorContext
            .Users
            .FirstOrDefaultAsync(e => e.TelegramId == user.Id, cancellationToken);

        if (userFromDb == null)
            _sheetEditorContext.Users.Add(new User
            {
                TelegramId = user.Id
            });

        await _sheetEditorContext.SaveChangesAsync(cancellationToken);

        await botClient.SendTextMessageAsync(update.Message!.Chat.Id,
            "Чтобы задать почту, введите команду '/setEmail {ваш gmail}'." +
            " Этой почте будет выдаваться роль writer для всех созданных таблицы",
            replyMarkup: new ForceReplyMarkup(),
            cancellationToken: cancellationToken
        );
    }
}