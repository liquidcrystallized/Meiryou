using Meiryou.Core.Models;
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
    private ITextParsingService _mockTextParsingService;
    private IWordService _mockWordService;
    private ReaderScreenViewModel _viewModel;
    
    [SetUp]
    public void SetUp()
    {
        _mockScreen = Substitute.For<IScreen>();
        _mockTextParsingServiceFactory = Substitute.For<ITextParsingServiceFactory>();
        _mockTextParsingService = Substitute.For<ITextParsingService>();
        _mockWordService = Substitute.For<IWordService>();
        _viewModel = new ReaderScreenViewModel(_mockScreen, _mockTextParsingServiceFactory, _mockWordService);
    }
    
    [Test]
    public void Constructor_InitializesWithEmptyWords()
    {
        Assert.That(_viewModel.Words, Is.Not.Null);
        Assert.That(_viewModel.Words, Is.Empty);
        Assert.That(_viewModel.IsPopupVisible, Is.False);
        Assert.That(_viewModel.SelectedWord, Is.Null);
    }
    
    [Test]
    public void SelectWordCommand_SetsSelectedWordAndTogglesPopup()
    {
        var testContent = CreateTestContent(LanguageType.Japanese, "空に消える");

        _mockTextParsingServiceFactory.GetService(LanguageType.Japanese).Returns(_mockTextParsingService);
        _mockTextParsingService.SegmentTextIntoWords(testContent.Content).Returns(["空", "に", "消える"]);
        _mockWordService.GetWordsByTextAsync(Arg.Any<IEnumerable<string>>())
            .Returns(Task.FromResult<IEnumerable<Word>>([]));
        _mockWordService.CreateWordAsync("空").Returns(Task.FromResult(new Word { Text = "空" }));
        _mockWordService.CreateWordAsync("に").Returns(Task.FromResult(new Word { Text = "に" }));
        _mockWordService.CreateWordAsync("消える").Returns(Task.FromResult(new Word { Text = "消える" }));

        var loadContentTask = _viewModel.LoadContent(testContent);
        loadContentTask.Wait();
        
        var firstWord = _viewModel.Words.FirstOrDefault(w => !w.IsSpace);
        Assert.That(firstWord, Is.Not.Null);
        
        _viewModel.SelectedWordCommand.Execute(firstWord).Subscribe();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(_viewModel.SelectedWord, Is.EqualTo(firstWord));
            Assert.That(_viewModel.IsPopupVisible, Is.True);
        }
    }
    
    [Test]
    public void SelectWordCommand_TogglesPopupOnSubsequentClicks()
    {
        var testContent = CreateTestContent(LanguageType.Japanese, "空に消える");
        
        _mockTextParsingServiceFactory.GetService(LanguageType.Japanese).Returns(_mockTextParsingService);
        _mockTextParsingService.SegmentTextIntoWords(testContent.Content).Returns(["空", "に", "消える"]);
        _mockWordService.GetWordsByTextAsync(Arg.Any<IEnumerable<string>>())
            .Returns(Task.FromResult<IEnumerable<Word>>([]));
        _mockWordService.CreateWordAsync("空").Returns(Task.FromResult(new Word { Text = "空" }));
        _mockWordService.CreateWordAsync("に").Returns(Task.FromResult(new Word { Text = "に" }));
        _mockWordService.CreateWordAsync("消える").Returns(Task.FromResult(new Word { Text = "消える" }));
        
        var loadContentTask = _viewModel.LoadContent(testContent);
        loadContentTask.Wait();
        
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
        var testContent = CreateTestContent(LanguageType.Japanese, "空に消える");
        
        _mockTextParsingServiceFactory.GetService(LanguageType.Japanese).Returns(_mockTextParsingService);
        _mockTextParsingService.SegmentTextIntoWords(testContent.Content).Returns(["空", "に", "消える"]);
        _mockWordService.GetWordsByTextAsync(Arg.Any<IEnumerable<string>>())
            .Returns(Task.FromResult<IEnumerable<Word>>([]));
        _mockWordService.CreateWordAsync("空").Returns(Task.FromResult(new Word { Text = "空" }));
        _mockWordService.CreateWordAsync("に").Returns(Task.FromResult(new Word { Text = "に" }));
        _mockWordService.CreateWordAsync("消える").Returns(Task.FromResult(new Word { Text = "消える" }));
        
        var loadContentTask = _viewModel.LoadContent(testContent);
        loadContentTask.Wait();
        
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
        var testContent = CreateTestContent(LanguageType.Japanese, "空に消える");
        
        _mockTextParsingServiceFactory.GetService(LanguageType.Japanese).Returns(_mockTextParsingService);
        _mockTextParsingService.SegmentTextIntoWords(testContent.Content).Returns(["空", "に", "消える"]);
        _mockWordService.GetWordsByTextAsync(Arg.Any<IEnumerable<string>>())
            .Returns(Task.FromResult<IEnumerable<Word>>([]));
        _mockWordService.CreateWordAsync("空").Returns(Task.FromResult(new Word { Text = "空" }));
        _mockWordService.CreateWordAsync("に").Returns(Task.FromResult(new Word { Text = "に" }));
        _mockWordService.CreateWordAsync("消える").Returns(Task.FromResult(new Word { Text = "消える" }));
        
        var loadContentTask = _viewModel.LoadContent(testContent);
        loadContentTask.Wait();
        
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

    private static ReadingContent CreateTestContent(LanguageType language, string content)
    {
        return new ReadingContent
        {
            Id = 1,
            Language = language,
            Title = "Test Content",
            Content = content
        };
    }
}