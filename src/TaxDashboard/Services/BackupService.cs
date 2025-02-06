using CommunityToolkit.Maui.Storage;

using LifeManagers.Data;
using LifeManagers.Data.Backup;

using Microsoft.Extensions.Options;

namespace TaxDashboard.Services;

public class BackupService(IBackupManager backupManager, IPeriodicBackuper periodicBackuper, IOptions<DataServicesOptions> options)
{
    private static readonly string[] AllowedFileTypes = [".db3"];
    private static readonly PickOptions FilePickerOptions = new()
    {
        PickerTitle = "Wybierz plik bazy danych (*.db3)",
        FileTypes = new(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, AllowedFileTypes },
            })
    };

    private readonly IBackupManager _backupManager = backupManager;
    private readonly IPeriodicBackuper _periodicBackuper = periodicBackuper;
    private readonly DataServicesOptions _options = options.Value;

    public Task<DateTime?> GetLastBackupTimeAsync() => _periodicBackuper.GetLastBackupDateTimeAsync();
    public TimeSpan? GetBackupPeriod() => _options.BackupPeriod;

    public async Task<BackupResult> LoadBackupFromFileAsyc()
    {

        FileResult? filePickerResult = await FilePicker.Default.PickAsync(FilePickerOptions);

        if (filePickerResult is not null)
        {
            if (filePickerResult.FileName.EndsWith("db3", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    await Task.Run(() => _backupManager.ReplaceDatabaseFile(filePickerResult.FullPath));
                }
                catch (Exception e)
                {
                    return new(false, e.Message);
                }
            }
            else
                return new(false, "Nieprawidłowy format pliku");
        }
        else
            return new(false, "Nie wybrano żadnego pliku");

        return new();
    }

    public async Task<BackupResult> MakeBackupAsync()
    {
        // HACK: Creating end deleting empty file to get FileSaver result path
        using MemoryStream blankStream = new([0]);
        FileSaverResult fileSaveResult = await FileSaver.Default.SaveAsync($"{DateTime.Today:yy-MM-dd}-backup.db3", blankStream);
        if(fileSaveResult.IsSuccessful)
        {
            File.Delete(fileSaveResult.FilePath);
            _backupManager.BackupDatabase(fileSaveResult.FilePath);
            return new();
        }
        return new(false, fileSaveResult.Exception.Message);
    }
}

public record BackupResult(bool IsSuccessful = true, string Message = "");