using System;
using ReactiveUI;

namespace Meiryou.ViewModels;

public class SplashScreenViewModel : ReactiveObject, IRoutableViewModel
{
    public IScreen HostScreen { get; set; }
    
    public bool IsLoading
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string StatusMessage
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public SplashScreenViewModel()
    {
        IsLoading = true;
        StatusMessage = "Loading...";
    }
}