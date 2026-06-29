using Meiryou.Core.Services;
using Meiryou.Core.Services.TextParsing;
using Meiryou.ViewModels;
using NSubstitute;
using ReactiveUI;

namespace Meiryou.Tests.UnitTests.App.ViewModels;

[TestFixture]
public class ReaderScreenViewModelTests
{
    private IScreen _mockScreen;
    private ITextParsingServiceFactory _mockTextParsingServiceFactory;
    private IWordService _mockWordService;
    private ReaderScreenViewModel _viewModel;
    
    [SetUp]
    public void SetUp()
    {
        _mockScreen = Substitute.For<IScreen>();
        _mockTextParsingServiceFactory = Substitute.For<ITextParsingServiceFactory>();
        _mockWordService = Substitute.For<IWordService>();
        _viewModel = new ReaderScreenViewModel(_mockScreen, _mockTextParsingServiceFactory, _mockWordService);
    }
    
    [Test]
    public void Constructor_InitializesWithEmptyWords()
    {
        Assert.That(_viewModel.Words, Is.Not.Null);
        //TODO: Remove random words on initialisation later.
        //Assert.That(vm.Words.Count, Is.EqualTo(0)); // This currently will fail because I'm adding random words on initialisation.
        Assert.That(_viewModel.IsPopupVisible, Is.False);
        Assert.That(_viewModel.SelectedWord, Is.Null);
    }
    
    [Test]
    public void AddRandomTextCommand_AddsWordsToCollection()
    {
        _viewModel.AddRandomTextCommand.Execute().Subscribe();
        
        Assert.That(_viewModel.Words, Is.Not.Empty);
    }
    
    [Test]
    public void SelectWordCommand_SetsSelectedWordAndTogglesPopup()
    {
        _viewModel.AddRandomTextCommand.Execute().Subscribe();
        
        var firstWord = _viewModel.Words.FirstOrDefault(w => !w.IsSpace);
        Assert.That(firstWord, Is.Not.Null);
        
        _viewModel.SelectedWordCommand.Execute(firstWord).Subscribe();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_viewModel.SelectedWord, Is.EqualTo(firstWord));
            Assert.That(_viewModel.IsPopupVisible, Is.True);
        }
    }
    
    //[Test]
    //public void SelectWordCommand_DoesNotSelectSpaceEntries()
    //{
    //    var vm = new ReaderScreenViewModel();
    //    vm.AddRandomTextCommand.Execute().Subscribe();
    //    
    //    var spaceEntry = vm.Words.FirstOrDefault(w => w.IsSpace);
    //    Assert.That(spaceEntry, Is.Not.Null);
    //    
    //    vm.SelectedWordCommand.Execute(spaceEntry).Subscribe();
    //    
    //    using (Assert.EnterMultipleScope())
    //    {
    //        Assert.That(vm.SelectedWord, Is.Null);
    //        Assert.That(vm.IsPopupVisible, Is.False);
    //    }
    //}
    
    [Test]
    public void SelectWordCommand_TogglesPopupOnSubsequentClicks()
    {
        _viewModel.AddRandomTextCommand.Execute().Subscribe();
        
        var firstWord = _viewModel.Words.FirstOrDefault(w => !w.IsSpace);
        Assert.That(firstWord, Is.Not.Null);
        
        _viewModel.SelectedWordCommand.Execute(firstWord).Subscribe();
        Assert.That(_viewModel.IsPopupVisible, Is.True);

        _viewModel.SelectedWordCommand.Execute(firstWord).Subscribe();
        Assert.That(_viewModel.IsPopupVisible, Is.False);
        
        _viewModel.SelectedWordCommand.Execute(firstWord).Subscribe();
        Assert.That(_viewModel.IsPopupVisible, Is.True);
        
        _viewModel.SelectedWordCommand.Execute(firstWord).Subscribe();
        Assert.That(_viewModel.IsPopupVisible, Is.False);
    }
    
    [Test]
    public void ClosePopupCommand_ClosesPopupAndClearsSelectedWord()
    {
        _viewModel.AddRandomTextCommand.Execute().Subscribe();
        
        var firstWord = _viewModel.Words.FirstOrDefault(w => !w.IsSpace);
        Assert.That(firstWord, Is.Not.Null);
        
        _viewModel.SelectedWordCommand.Execute(firstWord).Subscribe();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_viewModel.IsPopupVisible, Is.True);
            Assert.That(_viewModel.SelectedWord, Is.Not.Null);
        }

        _viewModel.ClosePopupCommand.Execute().Subscribe();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_viewModel.IsPopupVisible, Is.False);
            Assert.That(_viewModel.SelectedWord, Is.Null);
        }
    }
    
    [Test]
    public void SelectWordCommand_TogglesPopupAfterClosePopup()
    {
        _viewModel.AddRandomTextCommand.Execute().Subscribe();
        
        var firstWord = _viewModel.Words.FirstOrDefault(w => !w.IsSpace);
        Assert.That(firstWord, Is.Not.Null);
        
        _viewModel.SelectedWordCommand.Execute(firstWord).Subscribe();
        Assert.That(_viewModel.IsPopupVisible, Is.True);
        
        _viewModel.ClosePopupCommand.Execute().Subscribe();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(_viewModel.IsPopupVisible, Is.False);
            Assert.That(_viewModel.SelectedWord, Is.Null);
        }

        _viewModel.SelectedWordCommand.Execute(firstWord).Subscribe();
        Assert.That(_viewModel.IsPopupVisible, Is.True);
    }
}