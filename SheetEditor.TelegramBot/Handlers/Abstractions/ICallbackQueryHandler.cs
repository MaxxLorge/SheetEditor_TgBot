namespace SheetEditor.Handlers.Abstractions;

public interface ICallbackQueryHandler : IHandler
{
    public string CallbackKey { get; }
}