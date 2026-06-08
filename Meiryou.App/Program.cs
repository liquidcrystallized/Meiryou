using Avalonia;
using ReactiveUI.Avalonia;
using ReactiveUI.Avalonia.Splat;
using System;
using Meiryou.Core.Data;
using Meiryou.Core.Services;
using Meiryou.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Splat;

namespace Meiryou;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUIWithMicrosoftDependencyResolver(
                services =>
                {
                    services.AddDbContext<MeiryouDbContext>(options =>
                        options.UseSqlite("Data Source=meiryou.db"));

                    services.AddScoped<ITextImportService, TextImportService>();
                    services.AddScoped<IReadingContentService, ReadingContentService>();

                    services.AddScoped<MainWindowViewModel>();
                    services.AddScoped<MenuScreenViewModel>();
                    services.AddScoped<LibraryScreenViewModel>();
                    services.AddScoped<ReaderScreenViewModel>();
                    services.AddScoped<SettingsScreenViewModel>();

                    services.AddSingleton<IViewLocator, ViewLocator>();
                    
                    services.AddSingleton<IServiceProvider>(sp => sp);
                },
                withResolver: sp =>
                {
                    // Optional: access ServiceProvider
                });
}