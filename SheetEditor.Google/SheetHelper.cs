using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace SheetEditor.Google;

public interface ISheetHelper
{
    Task<Spreadsheet> CreateSpreadSheet(string title, string email);
    Task ChangeCell(string spreadsheetId, string cellExpression, string expression);

    Task<string> ReadCell(string spreadSheetId, string cell);
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

    public async Task ChangeCell(string spreadsheetId, string cellExpression, string expression)
    {
        var spreadSheet = await _sheetsService.Spreadsheets.Get(spreadsheetId).ExecuteAsync();

        var body = new ValueRange
        {
            Range = cellExpression,
            Values = new List<IList<object>>
            {
                new List<object>
                {
                    expression
                }
            }
        };
        var updateRequest = _sheetsService.Spreadsheets.Values.Update(body, spreadsheetId, cellExpression);
        updateRequest.ValueInputOption =
            SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
        await updateRequest.ExecuteAsync();
    }

    public async Task<string> ReadCell(string spreadSheetId, string cell)
    {
        var spreadSheet = await _sheetsService.Spreadsheets.Get(spreadSheetId).ExecuteAsync();

        var value = await _sheetsService.Spreadsheets.Values.Get(spreadSheetId, cell).ExecuteAsync();
        return value.Values?[0]?[0] as string;
    }
}