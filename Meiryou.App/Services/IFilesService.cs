using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace Meiryou.Services;

public interface IFilesService
{
    public Task<IStorageFile?> OpenFileAsync();
}