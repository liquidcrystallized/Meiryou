using System;
using Avalonia.Input;
using Meiryou.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Meiryou.Views;

//TODO: Try to get rid of most of the code behind.
public partial class ReaderScreenView : ReactiveUserControl<ReaderScreenViewModel>
{
    public ReaderScreenView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }

    private void ScrollViewer_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var viewModel = DataContext as ReaderScreenViewModel;
        viewModel?.ClosePopupCommand.Execute().Subscribe();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        e.Handled = true;
        
        if ((sender as Avalonia.Controls.Control)?.DataContext is not WordEntry wordEntry) 
            return;

        var viewModel = DataContext as ReaderScreenViewModel;
        viewModel?.SelectedWordCommand.Execute(wordEntry).Subscribe();
    }
}