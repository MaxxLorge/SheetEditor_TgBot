using SheetEditor.Data;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SheetEditor.Handlers.ReplyTo;

public class MailMessageHandler : IReplyToMessageHandler
{
    private readonly SheetEditorContext _sheetEditorContext;

    public MailMessageHandler(SheetEditorContext sheetEditorContext)
    {
        _sheetEditorContext = sheetEditorContext;
    }

    public string MessageKey => "Введите почту:";

    public async Task Process(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var telegramUser = update.Message.From;
        var user = _sheetEditorContext.Users.FirstOrDefault(e => e.TelegramId == telegramUser.Id);
        user.Email = update.Message.Text;
        _sheetEditorContext.Update(user);
        await _sheetEditorContext.SaveChangesAsync(cancellationToken);
        await botClient.SendTextMessageAsync(
            update.Message.Chat.Id,
            "Введите название таблицы",
            replyMarkup: new ForceReplyMarkup(),
            cancellationToken: cancellationToken);
    }
}