namespace SheetEditor.Data.Entities;

public class Spreadsheet
{
    public int Id { get; set; }

    public string? SpreadsheetId { get; set; }
    public string? Url { get; set; }
    public string? Title { get; set; }

    public int ApplicationUserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }
}