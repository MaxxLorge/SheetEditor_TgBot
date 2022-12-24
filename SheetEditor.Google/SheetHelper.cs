using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace SheetEditor.Google;

public interface ISheetHelper
{
    Task<Spreadsheet> CreateSpreadSheet(string title, string email);
}

public class SheetHelper : ISheetHelper
{
    private readonly SheetsService _sheetsService;
    private readonly DriveService _driveService;

    public SheetHelper(SheetsService sheetsService, DriveService driveService)
    {
        _sheetsService = sheetsService;
        _driveService = driveService;
    }

    public async Task<Spreadsheet> CreateSpreadSheet(string title, string email)
    {
        var newSpreadSheet = await _sheetsService.Spreadsheets.Create(new Spreadsheet
        {
            Properties = new SpreadsheetProperties
            {
                Title = title
            }
        }).ExecuteAsync();

        var permission = new Permission
        {
            Type = "user",
            Role = "writer",
            EmailAddress = email
        };

        await _driveService.Permissions
            .Create(permission, newSpreadSheet.SpreadsheetId)
            .ExecuteAsync();

        return newSpreadSheet;
    }
}