using System;
using Meiryou.Core.Services;
using Meiryou.Core.Services.TextParsing;
using Meiryou.Services;
using Meiryou.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Meiryou.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection services)
    {
        services.AddTransient<IFilesService, FilesService>();
        
        services.AddSingleton<ITextParsingServiceFactory, TextParsingServiceFactory>();
        services.AddSingleton<ITextParsingService, JapaneseTextParsingService>(_ => new JapaneseTextParsingService());
        
        services.AddScoped<IContentWordService, ContentWordService>();
        services.AddScoped<IReadingContentService, ReadingContentService>();
        services.AddScoped<IWordService, WordService>();

        services.AddSingleton<IScreen, MainWindowViewModel>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<SplashScreenViewModel>();
        services.AddTransient<LibraryScreenViewModel>();
        services.AddTransient<ReaderScreenViewModel>();
        services.AddTransient<SettingsScreenViewModel>();

        services.AddSingleton<IViewLocator, ViewLocator>();
                    
        services.AddSingleton<IServiceProvider>(sp => sp);
    }
}