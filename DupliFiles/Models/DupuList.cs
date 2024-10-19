using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
namespace DupliFiles.Models;
public class DupuList
{
    public DupuList(List<FileInfo> infos)
    {
        foreach (FileInfo info in infos)
        {
            var x = new File(info);
            x.DeleteEvent += DeleteItem;
            DuplicateFiles.Add(x);
        }
    }
    public ObservableCollection<File> DuplicateFiles { get; } = [];
    public void DeleteItem(object sender, FileEventArgs e)
    {
        DuplicateFiles.Remove(e.File);
    }
}
