using SheetEditor.Google;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SheetEditor;

public class UpdateHandler : IUpdateHandler
{
    private string _email;
    private string _title;
    private readonly ISheetHelper _sheetHelper;

    public UpdateHandler(ISheetHelper sheetHelper)
    {
        _sheetHelper = sheetHelper;
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

        if (messageText == "/start")
            await botClient.SendTextMessageAsync(chatId,
                "Введите вашу почту",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
            
        
        // Echo received message text
        var sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "You said:\n" + messageText,
            cancellationToken: cancellationToken);
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