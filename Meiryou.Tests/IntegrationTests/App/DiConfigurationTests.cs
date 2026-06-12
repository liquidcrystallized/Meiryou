using Meiryou.Core.Data;
using Meiryou.Core.Services;
using Meiryou.Extensions;
using Meiryou.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Meiryou.Tests.IntegrationTests.App;

public class MockScreen : IScreen
{
    public RoutingState Router { get; } = new();
}

[TestFixture]
public class DiConfigurationTests
{
    private IServiceProvider _services;

    [SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        
        services.AddDbContext<MeiryouDbContext>(options =>
            options.UseSqlite("DataSource=:memory:"));

        services.AddSingleton<IScreen, MockScreen>();
        services.AddCommonServices();

        _services = services.BuildServiceProvider();
    }

    [TearDown]
    public void TearDown()
    {
        if (_services is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    [Test]
    public void DbContext_ShouldBeRegistered()
    {
        var dbContext = _services.GetService<MeiryouDbContext>();
        
        Assert.That(dbContext, Is.Not.Null);
    }

    [Test]
    public void TextImportService_ShouldBeResolvable()
    {
        var service = _services.GetService<ITextImportService>();
        
        Assert.That(service, Is.Not.Null);
        Assert.That(service, Is.InstanceOf<ITextImportService>());
    }

    [Test]
    public void ReadingContentService_ShouldBeResolvable()
    {
        var service = _services.GetService<IReadingContentService>();
        
        Assert.That(service, Is.Not.Null);
        Assert.That(service, Is.InstanceOf<IReadingContentService>());
    }

    [Test]
    public void MainWindowViewModel_ShouldBeResolvable()
    {
        var viewModel = _services.GetService<MainWindowViewModel>();
        
        Assert.That(viewModel, Is.Not.Null);
        Assert.That(viewModel, Is.InstanceOf<MainWindowViewModel>());
    }

    [Test]
    public void MenuScreenViewModel_ShouldBeResolvable()
    {
        var viewModel = _services.GetService<MenuScreenViewModel>();
        
        Assert.That(viewModel, Is.Not.Null);
        Assert.That(viewModel, Is.InstanceOf<MenuScreenViewModel>());
        Assert.That(viewModel.HostScreen, Is.InstanceOf<MockScreen>());
    }

    [Test]
    public void LibraryScreenViewModel_ShouldBeResolvable()
    {
        var viewModel = _services.GetService<LibraryScreenViewModel>();
        
        Assert.That(viewModel, Is.Not.Null);
        Assert.That(viewModel, Is.InstanceOf<LibraryScreenViewModel>());
    }

    [Test]
    public void SettingsScreenViewModel_ShouldBeResolvable()
    {
        var viewModel = _services.GetService<SettingsScreenViewModel>();
        
        Assert.That(viewModel, Is.Not.Null);
        Assert.That(viewModel, Is.InstanceOf<SettingsScreenViewModel>());
    }
    
    [Test]
    public void ViewLocator_ShouldBeResolvable()
    {
        var viewLocator = _services.GetService<IViewLocator>();
        
        Assert.That(viewLocator, Is.Not.Null);
        Assert.That(viewLocator, Is.InstanceOf<ViewLocator>());
    }

    [Test]
    public void IServiceProvider_ShouldBeResolvable()
    {
        var serviceProvider = _services.GetService<IServiceProvider>();
        
        Assert.That(serviceProvider, Is.Not.Null);
        Assert.That(serviceProvider, Is.InstanceOf<IServiceProvider>());
    }
    
    [Test]
    public void AllViewModels_ShouldHaveParameterlessConstructorOrDependenciesResolvable()
    {
        Assert.DoesNotThrow(() => _services.GetRequiredService<MainWindowViewModel>());
        Assert.DoesNotThrow(() => _services.GetRequiredService<MenuScreenViewModel>());
        Assert.DoesNotThrow(() => _services.GetRequiredService<LibraryScreenViewModel>());
        Assert.DoesNotThrow(() => _services.GetRequiredService<ReaderScreenViewModel>());
        Assert.DoesNotThrow(() => _services.GetRequiredService<SettingsScreenViewModel>());
    }
}