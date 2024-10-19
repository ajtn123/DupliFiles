using Avalonia.Platform.Storage;
using DupliFiles.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Input;
namespace DupliFiles.ViewModels;
public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        OpenFolderCommand = ReactiveCommand.CreateFromTask(OpenFolder);
        LoadFilesCommand = ReactiveCommand.Create(LoadFiles);
        CompareCommand = ReactiveCommand.Create(Compare);
        SelectFilesInteraction = new();
    }
    public ObservableCollection<DupuList> DuplicateFiles { get; set; } = [];
    public ObservableCollection<Folder> Folders { get; set; } = [];
    public ObservableCollection<FileInfo> Files { get; set; } = [];
    public ObservableCollection<IStorageFolder> Preders { get; set; } = [];
    public ICommand OpenFolderCommand { get; }
    public ICommand LoadFilesCommand { get; }
    public ICommand CompareCommand { get; }
    public void RefreshFolder()
    {
        Folders.Clear();
        foreach (IStorageFolder folder in Preders)
        {
            Folder item = new(folder);
            Folders.Add(item);
        }
    }
    public async Task OpenFolder()
    {
        var folders = await SelectFilesInteraction.Handle("Hifumi Daisuki");
        foreach (var folder in folders)
            Preders.Add(folder);
        RefreshFolder();
    }
    public Interaction<string?, System.Collections.Generic.IReadOnlyList<IStorageFolder>> SelectFilesInteraction { get; }
    public void Compare()
    {
        DuplicateFiles.Clear();
        SHA256 sha256 = SHA256.Create();
        List<long> xe = [];
        foreach (var file in Files)
            xe.Add(file.Length);
        var duplicates = xe.Select((t, i) => new { Index = i, Length = t })
                           .GroupBy(g => g.Length)
                           .Where(g => g.Count() > 1);
        foreach (var dupu in duplicates)
        {
            List<string> hashs = [];
            foreach (var it in dupu)
            {
                using var stream = Files[it.Index].OpenRead();
                hashs.Add(System.Text.Encoding.UTF8.GetString(sha256.ComputeHash(stream)));
            }
            var dupuHashs = hashs.Select((t, i) => new { Index = i, Hash = t })
                                 .GroupBy(g => g.Hash)
                                 .Where(g => g.Count() > 1);
            foreach (var set in dupuHashs)
            {
                List<FileInfo> dupus = [];
                foreach (var item in set)
                {
                    dupus.Add(Files[dupu.ElementAt(item.Index).Index]);
                }
                DuplicateFiles.Add(new DupuList(dupus));
            }
        }
    }
    public void LoadFiles()
    {
        RefreshFolder();
        Files.Clear();
        foreach (var item in Folders)
            foreach (var file in item.ChildrenFiles)
                Files.Add(file);

    }
}
