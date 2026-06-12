using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;

namespace Meiryou.Services;

public class FilesService : IFilesService
{
    public async Task<IStorageFile?> OpenFileAsync()
    {
        // TODO: May not work on mobile maybe(?)
        var topLevel = TopLevel.GetTopLevel(Avalonia.Threading.Dispatcher.UIThread?.Invoke(() => 
            Application.Current?.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime)?.MainWindow);

        if (topLevel?.StorageProvider is null)
        {
            return null;
        }

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select a text file",
            AllowMultiple = false,
            FileTypeFilter = [ FilePickerFileTypes.TextPlain ]
        });

        return files.Count >= 1 ? files[0] : null;
    }
}