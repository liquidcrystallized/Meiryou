using Meiryou.Business;
using Meiryou.Services;
using Meiryou.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Meiryou.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddSingleton<IFileLocator, FileLocator>();
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddTransient<LibraryWindowViewModel>();
    }
}