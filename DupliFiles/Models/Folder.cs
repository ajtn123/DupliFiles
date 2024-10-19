using Avalonia.Platform.Storage;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
namespace DupliFiles.Models;
public class Folder
{
    public Folder(IStorageFolder folder)
    {
        Path = folder.Path;
        Name = folder.Name;
        DirectoryInfo inn = new(folder.TryGetLocalPath());
        foreach (var file in inn.GetFiles().Where(fi => (fi.Attributes & (FileAttributes.Hidden | FileAttributes.System)) == 0))
            ChildrenFiles.Add(file);
    }
    public string Name { get; set; }
    public Uri Path { get; set; }
    public ObservableCollection<FileInfo> ChildrenFiles { get; set; } = [];
}
