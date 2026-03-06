using System;

namespace Meiryou.ViewModels;

public class ReaderScreenViewModel : ScreenViewModelBase
{
    public string Message => "Done"; 
    
    public override bool CanNavigateNext
    {
        get => false; 
        protected set => throw new NotSupportedException();
    }

    public override bool CanNavigatePrevious
    {
        get => true; 
        protected set => throw new NotSupportedException();
    }
}