using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace SheetEditor.Google;

public interface IGoogleServiceBuilder
{
    T BuildSheetService<T>(string appName, string pathToSecrets)
        where T : BaseClientService, new();
}

public class GoogleServiceBuilder
{
    public static T Build<T>(string appName, string pathToSecrets)
        where T : BaseClientService, new()
    {
        var initializer = new BaseClientService.Initializer
        {
            HttpClientInitializer = GoogleCredential.FromFile(pathToSecrets)
                .CreateScoped(SheetsService.Scope.Drive, SheetsService.Scope.Spreadsheets)
        };
        var googleService = (T?)Activator.CreateInstance(typeof(T), initializer);
        if (googleService == null)
            throw new InvalidOperationException("Не удалось создать google-сервис");
        return googleService;
    }
}