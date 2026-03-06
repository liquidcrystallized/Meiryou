namespace Meiryou.ViewModels;

/// <summary>
/// An abstract class for enabling page navigation.
/// </summary>
public abstract class ScreenViewModelBase : ViewModelBase
{
    public abstract bool CanNavigateNext { get; protected set; }
    public abstract bool CanNavigatePrevious { get; protected set; }
}