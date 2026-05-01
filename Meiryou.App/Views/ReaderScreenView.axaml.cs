using System;
using Avalonia.Input;
using Meiryou.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Meiryou.Views;

public partial class ReaderScreenView : ReactiveUserControl<ReaderScreenViewModel>
{
    public ReaderScreenView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }

    //TODO: Get rid of this, don't want stuff in the code behind if possible.
    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if ((sender as Avalonia.Controls.Control)?.DataContext is not WordEntry wordEntry) 
            return;

        var viewModel = DataContext as ReaderScreenViewModel;
        viewModel?.SelectedWordCommand.Execute(wordEntry).Subscribe();
    }
}