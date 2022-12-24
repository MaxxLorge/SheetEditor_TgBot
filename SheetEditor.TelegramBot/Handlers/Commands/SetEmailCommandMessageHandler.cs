using Microsoft.EntityFrameworkCore;
using SheetEditor.Data;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = SheetEditor.Data.Entities.User;

namespace SheetEditor.Handlers.Commands;

public class SetEmailCommandMessageHandler : ICommandMessageHandler
{
    private readonly SheetEditorContext _context;

    public SetEmailCommandMessageHandler(SheetEditorContext context)
    {
        _context = context;
    }

    public string MessageKey => "/setEmail";

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

        var email = splited[1];

        var dbUser = await _context.Users
            .FirstOrDefaultAsync(e => e.TelegramId == message!.From!.Id, cancellationToken);
        if (dbUser == null)
        {
            _context.Users.Add(new User
            {
                TelegramId = message.From.Id,
                Email = email
            });
        }
        else
        {
            dbUser.Email = email;
            _context.Update(dbUser);
        }

        await _context.SaveChangesAsync(cancellationToken);

        await botClient.SendTextMessageAsync(chatId,
            "Почта была успешно установлена. Чтобы сменить почту, введите команду еще раз",
            cancellationToken: cancellationToken);
    }
}