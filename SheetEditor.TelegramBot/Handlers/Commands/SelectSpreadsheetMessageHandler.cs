using SheetEditor.Data;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SheetEditor.Handlers.Commands;

public class SelectSpreadsheetMessageHandler : MessageHandlerBase, IHaveHelpDescription
{
    public SelectSpreadsheetMessageHandler(SheetEditorContext context) : base(context)
    {
    }

    public string HelpDescription => "<b>select</b> - выбрать таблицу";
    public override string MessageKey => "select";

    protected override async Task Handle(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var spreadSheets = ApplicationUser.Spreadsheets;
        string message;
        var keyboardButtons = new List<InlineKeyboardButton[]>();
        if (spreadSheets.Count == 0)
        {
            message = "Таблицы отстствуют";
        }
        else
        {
            message = $"Ваши таблицы:{Environment.NewLine}";
            foreach (var spreadsheet in spreadSheets)
                keyboardButtons.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                    $"{spreadsheet.Title}({spreadsheet.SpreadsheetId})",
                    $"setSpreadsheet {spreadsheet.SpreadsheetId}")
                });
        }
        
        await SendMessage(message,
            ParseMode.Html,
            new InlineKeyboardMarkup(keyboardButtons),
            cancellationToken);
    }
}