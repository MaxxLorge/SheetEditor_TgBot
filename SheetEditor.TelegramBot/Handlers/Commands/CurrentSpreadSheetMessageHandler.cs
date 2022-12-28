using Microsoft.EntityFrameworkCore;
using SheetEditor.Data;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SheetEditor.Handlers.Commands;

public class CurrentSpreadSheetMessageHandler : MessageHandlerBase, IHaveHelpDescription
{
    public CurrentSpreadSheetMessageHandler(SheetEditorContext context) : base(context)
    {
    }

    public string HelpDescription => "<b>current</b> - текущая таблица";
    public override string MessageKey => "current";

    protected override async Task Handle(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var currentSpreadsheet = ApplicationUser.CurrentSpreadsheet;
        var spreadSheet = await Context.Spreadsheets
            .FirstOrDefaultAsync(e => e.SpreadsheetId == currentSpreadsheet,
                cancellationToken);
        if (spreadSheet == null)
        {
            await SendMessage("Текущая таблица не задана",
                cancellationToken: cancellationToken);
            return;
        }

        await SendMessage($"Выбрана таблица: <a href='{spreadSheet.Url}'>{spreadSheet.Title}</a>",
            cancellationToken: cancellationToken,
            parseMode: ParseMode.Html);
    }
}