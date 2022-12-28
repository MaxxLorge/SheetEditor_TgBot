using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SheetEditor;

public class UpdateHandler : IUpdateHandler
{
    private readonly IDictionary<string, CallbackQueryHandlerBase> _callbackQueryHandlers;
    private readonly IDictionary<string, MessageHandlerBase> _commandMessageHandlers;

    public UpdateHandler(IEnumerable<MessageHandlerBase> commandMessageHandlers,
        IEnumerable<CallbackQueryHandlerBase> callbackQueryHandlers)
    {
        _callbackQueryHandlers = callbackQueryHandlers
            .DistinctBy(k => k.CallbackKey)
            .ToDictionary(k => k.CallbackKey);
        // TODO: почему-то в контейнере два одинаковых хэндлера
        _commandMessageHandlers = commandMessageHandlers
            .DistinctBy(k => k.MessageKey)
            .ToDictionary(k => k.MessageKey);
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        try
        {
            if (update.Type == UpdateType.Message)
                await HandleTextMessage(botClient, update, cancellationToken);
            if (update.Type == UpdateType.CallbackQuery)
                await HandleCallbackQuery(botClient, update, cancellationToken);
        }
        catch
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Произошла ошибка",
                cancellationToken: cancellationToken);
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        Console.WriteLine(errorMessage);
        return Task.CompletedTask;
    }

    private async Task HandleTextMessage(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var messageText = update.Message!.Text;
        var key = messageText!.Split().First();
        if (!_commandMessageHandlers.ContainsKey(key))
        {
            Console.WriteLine($"Неизвестная команда: {messageText}");
            return;
        }

        await _commandMessageHandlers[key].Process(botClient, update, cancellationToken);
    }

    private async Task HandleCallbackQuery(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        var callbackQuery = update.CallbackQuery;
        var key = callbackQuery.Data.Split().First();
        if (!_callbackQueryHandlers.ContainsKey(key))
        {
            Console.WriteLine($"Неизвестный callback: {key}");
            return;
        }

        await _callbackQueryHandlers[key].Process(botClient, update, cancellationToken);
    }
}