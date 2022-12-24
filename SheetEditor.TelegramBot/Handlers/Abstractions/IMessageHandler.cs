namespace SheetEditor.Handlers.Abstractions;

public interface IMessageHandler : IHandler
{
    public string MessageKey { get; }
}