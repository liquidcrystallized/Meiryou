using System;

namespace Meiryou.ViewModels;

public class MenuScreenViewModel : ScreenViewModelBase
{
    public string Title => "Main Menu";

    public string Message => "Press \"Next\" to route to the next screen";

    public override bool CanNavigateNext
    {
        get => true; 
        protected set => throw new NotSupportedException();
    }

    public override bool CanNavigatePrevious
    {
        get => false; 
        protected set => throw new NotSupportedException();
    }
}