using System;
using Avalonia.Controls;
using Avalonia.Input;
using Meiryou.Core.Models;
using Meiryou.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Meiryou.Views;

public partial class LibraryScreenView : ReactiveUserControl<LibraryScreenViewModel>
{
    public LibraryScreenView()
    {
        this.WhenActivated(disposables => { });
        InitializeComponent();
    }

    private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        e.Handled = true;

        if (sender is not StackPanel stackPanel || stackPanel.DataContext is not ReadingContent content) 
            return;
        
        if (DataContext is LibraryScreenViewModel viewModel)
        {
            viewModel.SelectContentAndLoadReaderCommand.Execute(content).Subscribe();
        }
    }
}