using SheetEditor.Google;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace SheetEditor;

public class UpdateHandler : IUpdateHandler
{
    private readonly ISheetHelper _sheetHelper;
    private readonly IDictionary<string, IReplyToMessageHandler> _replyToMessageHandlers;
    private readonly IDictionary<string, ICommandMessageHandler> _commandMessageHandlers;

    public UpdateHandler(ISheetHelper sheetHelper,
        IEnumerable<ICommandMessageHandler> commandMessageHandlers,
        IEnumerable<IReplyToMessageHandler> replyToMessageHandlers)
    {
        _sheetHelper = sheetHelper;
        _replyToMessageHandlers = replyToMessageHandlers
            .DistinctBy(k => k.MessageKey)
            .ToDictionary(k => k.MessageKey);
        // TODO: почему-то в контейнере два одинаковых хэндлера
        _commandMessageHandlers = commandMessageHandlers
            .DistinctBy(k => k.MessageKey)
            .ToDictionary(k => k.MessageKey);
    }
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
            return;
        // Only process text messages
        if (message.Text is not { } messageText)
            return;
        var chatId = message.Chat.Id;
        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        var key = messageText.Split().First();
        if (!_commandMessageHandlers.ContainsKey(key))
        {
            Console.WriteLine($"Неизвестная команда: {messageText}");
            return;
        }

        await _commandMessageHandlers[key].Process(botClient, update, cancellationToken);
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

}