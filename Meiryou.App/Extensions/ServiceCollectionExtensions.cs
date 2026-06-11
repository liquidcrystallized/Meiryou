using System;
using Meiryou.Core.Services;
using Meiryou.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Meiryou.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection services)
    {
        services.AddScoped<ITextImportService, TextImportService>();
        services.AddScoped<IReadingContentService, ReadingContentService>();

        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<MenuScreenViewModel>();
        services.AddTransient<LibraryScreenViewModel>();
        services.AddTransient<ReaderScreenViewModel>();
        services.AddTransient<SettingsScreenViewModel>();

        services.AddSingleton<IViewLocator, ViewLocator>();
                    
        services.AddSingleton<IServiceProvider>(sp => sp);
    }
}