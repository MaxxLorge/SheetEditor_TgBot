using SheetEditor.Data;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SheetEditor.Handlers.Commands;

public class HelpCommandMessageHandler : CommandMessageHandlerBase
{
    private readonly IEnumerable<IHaveHelpDescription> _handlers;

    public HelpCommandMessageHandler(SheetEditorContext context,
        IEnumerable<IHaveHelpDescription> handlers) : base(context)
    {
        _handlers = handlers.DistinctBy(e => e.MessageKey);
    }

    public string HelpDescription => "/help - вызвать справку";
    public override string MessageKey => "/help";

    protected override async Task Handle(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var message = $"Справка:{Environment.NewLine}";
        message = _handlers
            .Aggregate(message, (current, handler) => current + $"{handler.HelpDescription}{Environment.NewLine}");

        await SendMessage(message,
            ParseMode.Html,
            cancellationToken);
    }
}