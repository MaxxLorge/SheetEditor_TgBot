using Microsoft.EntityFrameworkCore;
using SheetEditor.Data;
using SheetEditor.Handlers.Abstractions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SheetEditor.Handlers.Commands;

public class SetEmailMessageHandler : MessageHandlerBase, IHaveHelpDescription
{
    public SetEmailMessageHandler(SheetEditorContext context) : base(context)
    {
    }

    public string HelpDescription => "<b>setEmail {gmail_почта}</b> - задать почту. Эта почта будет иметь роль" +
                                     " writer для всех созданных таблиц";
    public override string MessageKey => "setEmail";

    protected override async Task Handle(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
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