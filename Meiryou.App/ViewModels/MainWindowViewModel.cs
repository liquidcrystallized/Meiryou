using System;
using System.Windows.Input;
using ReactiveUI;

namespace Meiryou.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        _currentScreen = _screens[0];

        IObservable<bool> canNavNext = this.WhenAnyValue(x => x.CurrentScreen.CanNavigateNext);
        IObservable<bool> canNavPrev = this.WhenAnyValue(x => x.CurrentScreen.CanNavigatePrevious);
        
        NavigateNextCommand = ReactiveCommand.Create(NavigateNext, canNavNext);
        NavigatePreviousCommand = ReactiveCommand.Create(NavigatePrevious, canNavPrev);
    }
    
    private readonly ScreenViewModelBase[] _screens =
    {
        new MenuScreenViewModel(),
        new LibraryScreenViewModel(),
        new ReaderScreenViewModel()
    };
    
    private ScreenViewModelBase _currentScreen;

    public ScreenViewModelBase CurrentScreen
    {
        get => _currentScreen;
        private set => this.RaiseAndSetIfChanged(ref _currentScreen, value);
    }
    
    public ICommand NavigateNextCommand { get; }

    private void NavigateNext()
    {
        int index = _screens.IndexOf(CurrentScreen) + 1;
        
        //TODO: Check if index is valid.
        CurrentScreen = _screens[index];
    }
    
    public ICommand NavigatePreviousCommand { get; }

    private void NavigatePrevious()
    {
        int index = _screens.IndexOf(CurrentScreen) - 1;
        
        CurrentScreen = _screens[index];
    }
}