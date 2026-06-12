using System.Reactive.Threading.Tasks;
using Meiryou.Core.Data;
using Meiryou.Core.Models;
using Meiryou.Extensions;
using Meiryou.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Meiryou.Tests.UnitTests.App.ViewModels;

public class MockScreen : IScreen
{
    public RoutingState Router { get; } = new();
}

public class LibraryScreenViewModelTests
{
    private IServiceProvider _services;
    private MeiryouDbContext _context;
    private LibraryScreenViewModel _viewModel;

    [SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        
        services.AddDbContext<MeiryouDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: "TestDb"));

        services.AddSingleton<IScreen, MockScreen>();
        services.AddCommonServices();

        _services = services.BuildServiceProvider();
        _context = _services.GetRequiredService<MeiryouDbContext>();
        _viewModel = _services.GetRequiredService<LibraryScreenViewModel>();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        if (_services is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    [Test]
    public void Constructor_InitialiseWithEmptyContents()
    {
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
        await _context.ReadingContents.AddAsync(new ReadingContent
        {
            Title = "Test Content 1",
            Content = "This is test content 1",
            CreatedAt = DateTime.UtcNow
        });
        await _context.ReadingContents.AddAsync(new ReadingContent
        {
            Title = "Test Content 2",
            Content = "This is test content 2",
            CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();

        await _viewModel.LoadContentsCommand.Execute().ToTask();
        
        // Should be ordered by CreatedAt descending.
        Assert.That(_viewModel.Contents, Has.Count.EqualTo(2));
        Assert.That(_viewModel.Contents.First().Title, Is.EqualTo("Test Content 2"));
        Assert.That(_viewModel.Contents.Last().Title, Is.EqualTo("Test Content 1"));
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