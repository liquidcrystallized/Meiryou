using System;
using ReactiveUI;

namespace Meiryou.ViewModels;

public class MenuScreenViewModel : ReactiveObject, IRoutableViewModel
{
    public string Title => "MenuScreenViewModel";

    public string Message => "Press \"Next\" to add another \"MainMenuViewModel\" to the ReactUI NavigationStack";

    public IScreen HostScreen { get; set; }
    
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public MenuScreenViewModel(IScreen screen) => HostScreen = screen;
}