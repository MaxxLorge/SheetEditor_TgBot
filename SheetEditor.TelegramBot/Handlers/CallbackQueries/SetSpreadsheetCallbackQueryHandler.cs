using SheetEditor.Data;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SheetEditor.Handlers.CallbackQueries;

public class SetSpreadsheetCallbackQueryHandler : CallbackQueryHandlerBase
{
    protected override async Task Handle(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var spreadsheetId = CallbackArguments[1];
        var spreadSheet = ApplicationUser.Spreadsheets
            .FirstOrDefault(e => e.SpreadsheetId == spreadsheetId);

        ApplicationUser.CurrentSpreadsheet = spreadsheetId;
        Context.Users.Update(ApplicationUser);
        await Context.SaveChangesAsync(cancellationToken);

        await botClient.SendTextMessageAsync(
            CallbackQuery.Message.Chat.Id,
            $"Вы выбрали <a href='{spreadSheet.Url}'>таблицу</a>",
            ParseMode.Html);
    }

    public override string CallbackKey => "setSpreadsheet";

    public SetSpreadsheetCallbackQueryHandler(SheetEditorContext context) : base(context)
    {
    }
}