using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Meiryou.Extensions;
using Meiryou.ViewModels;
using Meiryou.Views;
using Microsoft.Extensions.DependencyInjection;
using Splat;

namespace Meiryou;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = Locator.Current.GetService<IServiceProvider>();

        if (services is null)
        {
            throw new InvalidOperationException("Dependency Injection container not configured.");
        }
        
        var viewModel = services.GetRequiredService<MainWindowViewModel>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}