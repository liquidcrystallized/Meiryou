using Meiryou.Core.Data;
using Meiryou.Core.Services;
using Meiryou.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Meiryou.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddDbContext<MeiryouDbContext>(options =>
            options.UseSqlite("Data Source=meiryou.db"));
       
        collection.AddScoped<ITextImportService, TextImportService>();
        collection.AddScoped<IReadingContentService, ReadingContentService>();
        
        collection.AddScoped<MainWindowViewModel>();
        collection.AddScoped<MenuScreenViewModel>();
        collection.AddScoped<LibraryScreenViewModel>();
        collection.AddScoped<ReaderScreenViewModel>();
        collection.AddScoped<SettingsScreenViewModel>();
    }
}