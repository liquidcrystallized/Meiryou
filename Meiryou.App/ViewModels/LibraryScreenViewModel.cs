using System;
using System.ComponentModel.DataAnnotations;
using ReactiveUI;

namespace Meiryou.ViewModels;

public class LibraryScreenViewModel : ScreenViewModelBase
{
    public LibraryScreenViewModel()
    {
        this.WhenAnyValue(x => x.MailAddress, x => x.Password)
            .Subscribe(_ => UpdateCanNavigateNext());
    }
    
    private string? _mailAddress;
    private string? _password;
    private bool _canNavigateNext;

    [Required]
    [EmailAddress]
    public string? MailAddress
    {
        get => _mailAddress;
        set => this.RaiseAndSetIfChanged(ref _mailAddress, value);
    }

    public string? Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public override bool CanNavigateNext
    {
        get => _canNavigateNext;
        protected set => this.RaiseAndSetIfChanged(ref _canNavigateNext, value);
    }
    
    public override bool CanNavigatePrevious
    {
        get => true;
        protected set => throw new NotSupportedException();
    }

    private void UpdateCanNavigateNext()
    {
        CanNavigateNext =
            !string.IsNullOrEmpty(_mailAddress)
            && _mailAddress.Contains("@")
            && !string.IsNullOrEmpty(_password);
    }
}