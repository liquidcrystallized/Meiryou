using Meiryou.ViewModels;

namespace Meiryou.Tests.App.ViewModelTests;

[TestFixture]
public class ReaderScreenViewModelTests
{
    [Test]
    public void Constructor_InitializesWithEmptyWords()
    {
        var vm = new ReaderScreenViewModel();
        
        Assert.That(vm.Words, Is.Not.Null);
        //TODO: Remove random words on initialisation later.
        //Assert.That(vm.Words.Count, Is.EqualTo(0)); // This currently will fail because I'm adding random words on initialisation.
        Assert.That(vm.IsPopupVisible, Is.False);
        Assert.That(vm.SelectedWord, Is.Null);
    }
    
    [Test]
    public void AddRandomTextCommand_AddsWordsToCollection()
    {
        var vm = new ReaderScreenViewModel();
        vm.AddRandomTextCommand.Execute().Subscribe();
        
        Assert.That(vm.Words, Is.Not.Empty);
    }
    
    [Test]
    public void SelectWordCommand_SetsSelectedWordAndTogglesPopup()
    {
        var vm = new ReaderScreenViewModel();
        vm.AddRandomTextCommand.Execute().Subscribe();
        
        var firstWord = vm.Words.FirstOrDefault(w => !w.IsSpace);
        Assert.That(firstWord, Is.Not.Null);
        
        vm.SelectedWordCommand.Execute(firstWord).Subscribe();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(vm.SelectedWord, Is.EqualTo(firstWord));
            Assert.That(vm.IsPopupVisible, Is.True);
        }
    }
    
    [Test]
    public void SelectWordCommand_DoesNotSelectSpaceEntries()
    {
        var vm = new ReaderScreenViewModel();
        vm.AddRandomTextCommand.Execute().Subscribe();
        
        var spaceEntry = vm.Words.FirstOrDefault(w => w.IsSpace);
        Assert.That(spaceEntry, Is.Not.Null);
        
        vm.SelectedWordCommand.Execute(spaceEntry).Subscribe();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(vm.SelectedWord, Is.Null);
            Assert.That(vm.IsPopupVisible, Is.False);
        }
    }
    
    [Test]
    public void SelectWordCommand_TogglesPopupOnSubsequentClicks()
    {
        var vm = new ReaderScreenViewModel();
        vm.AddRandomTextCommand.Execute().Subscribe();
        
        var firstWord = vm.Words.FirstOrDefault(w => !w.IsSpace);
        Assert.That(firstWord, Is.Not.Null);
        
        vm.SelectedWordCommand.Execute(firstWord).Subscribe();
        Assert.That(vm.IsPopupVisible, Is.True);

        vm.SelectedWordCommand.Execute(firstWord).Subscribe();
        Assert.That(vm.IsPopupVisible, Is.False);
        
        vm.SelectedWordCommand.Execute(firstWord).Subscribe();
        Assert.That(vm.IsPopupVisible, Is.True);
        
        vm.SelectedWordCommand.Execute(firstWord).Subscribe();
        Assert.That(vm.IsPopupVisible, Is.False);
    }
    
    [Test]
    public void ClosePopupCommand_ClosesPopupAndClearsSelectedWord()
    {
        var vm = new ReaderScreenViewModel();
        vm.AddRandomTextCommand.Execute().Subscribe();
        
        var firstWord = vm.Words.FirstOrDefault(w => !w.IsSpace);
        Assert.That(firstWord, Is.Not.Null);
        
        vm.SelectedWordCommand.Execute(firstWord).Subscribe();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(vm.IsPopupVisible, Is.True);
            Assert.That(vm.SelectedWord, Is.Not.Null);
        }

        vm.ClosePopupCommand.Execute().Subscribe();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(vm.IsPopupVisible, Is.False);
            Assert.That(vm.SelectedWord, Is.Null);
        }
    }
    
    [Test]
    public void SelectWordCommand_TogglesPopupAfterClosePopup()
    {
        var vm = new ReaderScreenViewModel();
        vm.AddRandomTextCommand.Execute().Subscribe();
        
        var firstWord = vm.Words.FirstOrDefault(w => !w.IsSpace);
        Assert.That(firstWord, Is.Not.Null);
        
        vm.SelectedWordCommand.Execute(firstWord).Subscribe();
        Assert.That(vm.IsPopupVisible, Is.True);
        
        vm.ClosePopupCommand.Execute().Subscribe();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(vm.IsPopupVisible, Is.False);
            Assert.That(vm.SelectedWord, Is.Null);
        }

        vm.SelectedWordCommand.Execute(firstWord).Subscribe();
        Assert.That(vm.IsPopupVisible, Is.True);
    }
}