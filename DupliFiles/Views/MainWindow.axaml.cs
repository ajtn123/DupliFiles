using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using DupliFiles.ViewModels;
using ReactiveUI;
using System.Threading.Tasks;

namespace DupliFiles.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            d(ViewModel.SelectFilesInteraction.RegisterHandler(this.InteractionHandler));
        });
    }

    private async Task InteractionHandler(InteractionContext<string?, System.Collections.Generic.IReadOnlyList<IStorageFolder>> context)
    {
        // Get our parent top level control in order to get the needed service (in our sample the storage provider. Can also be the clipboard etc.)
        var topLevel = TopLevel.GetTopLevel(this);
        var storageFolders = await topLevel!.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions()
            {
                AllowMultiple = true,
                Title = context.Input
            });
        context.SetOutput(storageFolders);
    }
}