using Google.Apis.Sheets.v4.Data;
using Microsoft.EntityFrameworkCore;
using SheetEditor.Data;
using SheetEditor.Google;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SheetEditor.Handlers.Commands;

public class NewSpreadSheetCommandMessageHandler : CommandMessageHandlerBase
{
    private readonly ISheetHelper _sheetHelper;

    public NewSpreadSheetCommandMessageHandler(SheetEditorContext context, ISheetHelper sheetHelper) : base(context)
    {
        _sheetHelper = sheetHelper;
    }

    public override string MessageKey => "/new";

    public override async Task Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (MessageWords.Length != 2)
        {
            await SendMessage(
                "Неверный формат сообщения",
                cancellationToken: cancellationToken);
            return;
        }

        var dbUser = await Context.Users
            .FirstOrDefaultAsync(e => e.TelegramId == TelegramUser.Id, cancellationToken);
        if (dbUser?.Email == null)
        {
            await SendMessage(
                "Укажите почту командой /setEmail",
                cancellationToken: cancellationToken);
            return;
        }

        var title = MessageWords[1];
        var spreadSheet = default(Spreadsheet);
        try
        {
            spreadSheet = await _sheetHelper.CreateSpreadSheet(title, dbUser.Email);
        }
        catch
        {
            await SendMessage(
                "Не удалось создать таблицу.",
                cancellationToken: cancellationToken);
        }

        await SendMessage(
            $"Была создана таблица по <a href='{spreadSheet.SpreadsheetUrl}'>адресу</a>",
            ParseMode.Html,
            cancellationToken: cancellationToken);

        Context.Spreadsheets.Add(new Data.Entities.Spreadsheet
        {
            Title = title,
            Url = spreadSheet.SpreadsheetUrl,
            SpreadsheetId = spreadSheet.SpreadsheetId
        });
        await Context.SaveChangesAsync(cancellationToken);
    }
}