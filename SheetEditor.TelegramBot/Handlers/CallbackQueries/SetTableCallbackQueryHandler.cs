using SheetEditor.Data;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SheetEditor.Handlers.CallbackQueries;

public class SetTableCallbackQueryHandler : CallbackQueryHandlerBase
{
    protected override async Task Handle(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var spreadsheetId = CallbackArguments[1];
        var spreadSheet = ApplicationUser.Spreadsheets
            .FirstOrDefault(e => e.SpreadsheetId == spreadsheetId);

        if (spreadSheet == null)
            throw new NotImplementedException();

        ApplicationUser.CurrentSpreadsheet = spreadsheetId;
        Context.Users.Update(ApplicationUser);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public override string CallbackKey => "setSpreadsheet";

    public SetTableCallbackQueryHandler(SheetEditorContext context) : base(context)
    {
    }
}