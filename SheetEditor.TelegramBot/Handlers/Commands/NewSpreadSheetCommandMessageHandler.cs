using Google.Apis.Sheets.v4.Data;
using Microsoft.EntityFrameworkCore;
using SheetEditor.Data;
using SheetEditor.Google;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SheetEditor.Handlers.Commands;

public class NewSpreadSheetCommandMessageHandler : ICommandMessageHandler
{
    private readonly SheetEditorContext _context;
    private readonly ISheetHelper _sheetHelper;

    public NewSpreadSheetCommandMessageHandler(SheetEditorContext context, ISheetHelper sheetHelper)
    {
        _context = context;
        _sheetHelper = sheetHelper;
    }

    public string MessageKey => "/new";

    public async Task Process(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;
        var messageText = message?.Text;
        var chatId = message?.Chat.Id;
        if (messageText == null || chatId == null)
            throw new InvalidOperationException();
        var splited = messageText.Split(' ', StringSplitOptions.TrimEntries);

        if (splited.Length != 2)
        {
            await botClient.SendTextMessageAsync(chatId,
                "Неверный формат сообщения",
                cancellationToken: cancellationToken);
            return;
        }

        var dbUser = await _context.Users
            .FirstOrDefaultAsync(e => e.TelegramId == message!.From!.Id, cancellationToken);
        if (dbUser?.Email == null)
        {
            await botClient.SendTextMessageAsync(chatId,
                "Укажите почту командой /setEmail",
                cancellationToken: cancellationToken);
            return;
        }

        var title = splited[1];
        var spreadSheet = default(Spreadsheet);
        try
        {
            spreadSheet = await _sheetHelper.CreateSpreadSheet(title, dbUser.Email);
        }
        catch
        {
            await botClient.SendTextMessageAsync(chatId,
                "Не удалось создать таблицу.",
                cancellationToken: cancellationToken);
        }

        await botClient.SendTextMessageAsync(chatId,
            $"Была создана таблица по <a href='{spreadSheet.SpreadsheetUrl}'>адресу</a>",
            ParseMode.Html,
            cancellationToken: cancellationToken);

        _context.Spreadsheets.Add(new Data.Entities.Spreadsheet
        {
            Title = title,
            Url = spreadSheet.SpreadsheetUrl,
            SpreadsheetId = spreadSheet.SpreadsheetId
        });
        await _context.SaveChangesAsync(cancellationToken);
    }
}