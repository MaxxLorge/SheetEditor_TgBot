namespace SheetEditor.Data.Entities;

public class User
{
    public int Id { get; set; }
    public long? TelegramId { get; set; }
    public string? Email { get; set; }

    public IReadOnlyCollection<Spreadsheet> Spreadsheets { get; set; } = new List<Spreadsheet>();
}