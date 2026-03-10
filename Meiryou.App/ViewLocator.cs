using System;
using Meiryou.ViewModels;
using Meiryou.Views;
using ReactiveUI;

namespace Meiryou;

public class ViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T? viewModel, string? contract = null) => viewModel switch
    {
        MenuScreenViewModel context => new MenuScreenView { DataContext = context },
        LibraryScreenViewModel context => new LibraryScreenView { DataContext = context },
        SettingsScreenViewModel context => new SettingsScreenView { DataContext =  context },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}