using SheetEditor.Data;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SheetEditor.Handlers.Abstractions;

public abstract class CallbackQueryHandlerBase : HandlerBase, ICallbackQueryHandler
{
    public async Task Process(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await SetCommon(botClient, update, cancellationToken);
        CallbackQuery = update.CallbackQuery;
        await Handle(botClient, update, cancellationToken);
    }

    protected abstract Task Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

    public abstract string CallbackKey { get; }

    protected CallbackQuery CallbackQuery { get; private set; }
    protected User TelegramUser => CallbackQuery.From;
    protected string[] CallbackArguments => CallbackQuery.Data.Split();

    protected CallbackQueryHandlerBase(SheetEditorContext context) : base(context)
    {
    }

    protected override User GetTelegramUser(Update update)
    {
        return update.CallbackQuery.From;
    }
}