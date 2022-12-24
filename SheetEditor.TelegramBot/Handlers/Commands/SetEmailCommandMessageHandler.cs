using Microsoft.EntityFrameworkCore;
using SheetEditor.Data;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SheetEditor.Handlers.Commands;

public class SetEmailCommandMessageHandler : CommandMessageHandlerBase
{
    public SetEmailCommandMessageHandler(SheetEditorContext context) : base(context)
    {
    }

    public override string MessageKey => "setEmail";

    public override async Task Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (MessageWords.Length != 2)
        {
            await botClient.SendTextMessageAsync(ChatId,
                "Неверный формат сообщения",
                cancellationToken: cancellationToken);
            return;
        }

        var email = MessageWords[1];

        var dbUser = await Context.Users
            .FirstOrDefaultAsync(e => e.TelegramId == TelegramUser.Id, cancellationToken);
        dbUser.Email = email;
        await Context.SaveChangesAsync(cancellationToken);

        await SendMessage("Почта была успешно установлена. Чтобы сменить почту, введите команду еще раз",
            cancellationToken: cancellationToken);
    }
}