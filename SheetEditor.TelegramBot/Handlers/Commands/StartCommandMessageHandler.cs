using SheetEditor.Data;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SheetEditor.Handlers.Commands;

public class StartCommandMessageHandler : CommandMessageHandlerBase
{
    public StartCommandMessageHandler(SheetEditorContext sheetEditorContext) : base(sheetEditorContext)
    {
        
    }

    public override string MessageKey => "/start";

    public override async Task Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await SendMessage("Чтобы задать почту, введите команду '/setEmail {ваш gmail}'." +
                          " Этой почте будет выдаваться роль writer для всех созданных таблицы",
            cancellationToken: cancellationToken
        );
    }
}