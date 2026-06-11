using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Meiryou.Services;

public class FilesService : IFilesService
{
    private readonly Window _target;

    public FilesService(Window target)
    {
        _target = target;
    }
    
    public async Task<IStorageFile?> OpenFileAsync()
    {
        var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select a text file",
            AllowMultiple = false,
            FileTypeFilter = [ FilePickerFileTypes.TextPlain ]
        });

        return files.Count >= 1 ? files[0] : null;
    }
}