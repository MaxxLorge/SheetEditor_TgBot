using Microsoft.EntityFrameworkCore;
using SheetEditor.Data;
using SheetEditor.Google;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SheetEditor.Handlers.Commands;

public class ReadCellMessageHandler : MessageHandlerBase, IHaveHelpDescription
{
    private readonly ISheetHelper _sheetHelper;

    public ReadCellMessageHandler(SheetEditorContext context, ISheetHelper sheetHelper) : base(context)
    {
        _sheetHelper = sheetHelper;
    }

    public string HelpDescription => "<b>readCell {индекс_ячейки}</b> - прочитать значение из ячейки";
    public override string MessageKey => "readCell";

    protected override async Task Handle(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (MessageWords.Length != 2)
        {
            await SendMessage("Неверный формат команды", cancellationToken: cancellationToken);
            return;
        }

        var cell = MessageWords[1];
        var spreadSheet = await Context.Spreadsheets
            .FirstOrDefaultAsync(e => e.SpreadsheetId == ApplicationUser.CurrentSpreadsheet, cancellationToken);

        var value = await _sheetHelper.ReadCell(spreadSheet.SpreadsheetId, cell);

        await SendMessage(value ?? "значение отсутствует", cancellationToken: cancellationToken);
    }
}