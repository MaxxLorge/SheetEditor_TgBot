using SheetEditor.Data;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SheetEditor.Handlers.Commands;

public class StartMessageHandler : MessageHandlerBase
{
    public StartMessageHandler(SheetEditorContext sheetEditorContext) : base(sheetEditorContext)
    {
        
    }
    
    public override string MessageKey => "/start";

    protected override async Task Handle(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        await SendMessage("Для справки по командам наберите /help",
            cancellationToken: cancellationToken
        );
    }
}