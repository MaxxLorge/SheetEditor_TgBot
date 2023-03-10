using Microsoft.EntityFrameworkCore;
using SheetEditor.Data;
using SheetEditor.Google;
using SheetEditor.Handlers.Abstractions;
using SheetEditor.StaticData;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SheetEditor.Handlers.Commands;

public class ChangeCellMessageHandler : MessageHandlerBase, IHaveHelpDescription
{
    private readonly ISheetHelper _sheetHelper;

    public ChangeCellMessageHandler(SheetEditorContext context,
        ISheetHelper sheetHelper) : base(context)
    {
        _sheetHelper = sheetHelper;
    }

    public string HelpDescription =>
        "<b>setCell {индекс ячейки} \"{выражение}\"</b> - изменяет значение в заданной ячейке." +
        " Обратите внимание, что выражение должно быть внутри двойных кавычек. " +
        "С помощью команды можно редактировать только одну ячейку";
    public override string MessageKey => "setCell";

    protected override async Task Handle(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (MessageWords.Length < 3)
        {
            await SendMessage("Неверный формат команды", cancellationToken: cancellationToken);
            return;
        }

        var cell = MessageWords[1];
        var expression = RegularExpressions.CharactersBetweenQuotes().Match(MessageText).Value;
        var spreadSheet = await Context.Spreadsheets
            .FirstOrDefaultAsync(e => e.SpreadsheetId == ApplicationUser.CurrentSpreadsheet,
                cancellationToken);
        if (spreadSheet == null)
        {
            await SendMessage("Выберите таблицу для редактирования с помощью команды select",
                cancellationToken: cancellationToken);
            return;
        }

        await _sheetHelper.ChangeCell(spreadSheet.SpreadsheetId!, cell, expression);
        await SendMessage("Значение установлено", cancellationToken: cancellationToken);
    }
}