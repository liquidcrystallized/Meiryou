using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.NUnit;
using Meiryou.ViewModels;
using Meiryou.Views;

namespace Meiryou.Tests.Acceptance;

public class ReaderEndToEndTest
{
    private ReaderWindow _readerWindow;
    private ApplicationDatabase _applicationDatabase;
    
    [SetUp]
    public void Setup()
    {
        _readerWindow = new ReaderWindow
        {
            DataContext = new ReaderWindowViewModel()
        };
    }

    [AvaloniaTest]
    public void ReaderLoadsReadingContentAndCloses()
    {
        _readerWindow.Show();
        _readerWindow.LoadFile("test.txt");
        _readerWindow.Close();
    }
}
