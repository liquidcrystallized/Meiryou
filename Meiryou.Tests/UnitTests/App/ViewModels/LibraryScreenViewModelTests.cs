using System.Reactive.Threading.Tasks;
using Avalonia.Platform.Storage;
using Meiryou.Core.Models;
using Meiryou.Core.Services;
using Meiryou.Services;
using Meiryou.ViewModels;
using NSubstitute;
using ReactiveUI;

namespace Meiryou.Tests.UnitTests.App.ViewModels;

public class LibraryScreenViewModelTests
{
    private IScreen _mockScreen;
    private IFilesService _mockFilesService;
    private IReadingContentService _mockReadingContentService;
    private LibraryScreenViewModel _viewModel;

    [SetUp]
    public void SetUp()
    {
        _mockScreen = Substitute.For<IScreen>();
        _mockFilesService = Substitute.For<IFilesService>();
        _mockReadingContentService = Substitute.For<IReadingContentService>();

        _viewModel = new LibraryScreenViewModel(
            _mockScreen,
            _mockFilesService,
            _mockReadingContentService);
    }

    [Test]
    public void Constructor_InitialiseWithEmptyContents()
    {
        _mockReadingContentService.GetAllContentsAsync().Returns(Task.FromResult<IEnumerable<ReadingContent>>([]));
        
        Assert.That(_viewModel.Contents, Is.Not.Null);
        Assert.That(_viewModel.Contents, Is.Empty);
    }

    [Test]
    public void Constructor_SetIsLoadingToTrue_ThenFalse()
    {
        Task.Delay(100).Wait();
        
        Assert.That(_viewModel.IsLoadingContents, Is.False);
    }

    [Test]
    public async Task LoadContentsCommand_LoadsContentsFromService()
    {
        // Real service orders result by descending (newest content first).
        var expectedContents = new List<ReadingContent>
        {
            new() { Title = "Test Content 2", Content = "This is test content 2", CreatedAt = DateTime.UtcNow },
            new() { Title = "Test Content 1", Content = "This is test content 1", CreatedAt = DateTime.UtcNow }
        }.AsReadOnly();

        _mockReadingContentService.GetAllContentsAsync().Returns(Task.FromResult<IEnumerable<ReadingContent>>(expectedContents));

        await _viewModel.LoadContentsCommand.Execute().ToTask();
        
        Assert.That(_viewModel.Contents, Has.Count.EqualTo(2));
        Assert.That(_viewModel.Contents.First().Title, Is.EqualTo("Test Content 2"));
        Assert.That(_viewModel.Contents.First().Content, Is.EqualTo("This is test content 2"));
        Assert.That(_viewModel.Contents.Last().Title, Is.EqualTo("Test Content 1"));
        Assert.That(_viewModel.Contents.Last().Content, Is.EqualTo("This is test content 1"));
    }

    [Test]
    public async Task LoadContentsCommand_SetsLoadingStateCorrectly()
    {
        bool wasLoading = false;
        bool isLoadingAfter = false;

        var subscription = _viewModel
            .WhenAnyValue(vm => vm.IsLoadingContents)
            .Subscribe(value =>
            {
                if (value)
                {
                    wasLoading = true;
                }

                isLoadingAfter = value;
            });

        await _viewModel.LoadContentsCommand.Execute().ToTask();
        
        Assert.That(wasLoading, Is.True);
        Assert.That(isLoadingAfter, Is.False);

        subscription.Dispose();
    }

    [Test]
    public async Task ImportTextCommand_SetsImportingStateCorrectly()
    {
        _mockFilesService.OpenFileAsync().Returns(Task.FromResult<IStorageFile?>(null));
        
        bool wasImporting = false;
        bool isImportingAfter = false;

        var subscription = _viewModel
            .WhenAnyValue(vm => vm.IsImportingText)
            .Subscribe(value =>
            {
                if (value)
                {
                    wasImporting = true;
                }

                isImportingAfter = value;
            });

        await _viewModel.ImportTextCommand.Execute().ToTask();

        Assert.That(wasImporting, Is.True);
        Assert.That(isImportingAfter, Is.False);

        subscription.Dispose();
    }
}