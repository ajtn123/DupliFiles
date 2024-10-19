using ReactiveUI;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace DupliFiles.Models;

public class File
{
    public File(FileInfo info)
    {
        FileInfo = info;
        OpenCommand = ReactiveCommand.Create(Open);
        ShowCommand = ReactiveCommand.Create(Show);
        DeleteCommand = ReactiveCommand.Create(Delete);

    }

    public delegate void FileEventHandler(object sender, FileEventArgs e);
    public event FileEventHandler DeleteEvent;
    protected virtual void RaiseDeleteEvent()
    {
        DeleteEvent?.Invoke(this, new FileEventArgs(this));
    }

    public FileInfo FileInfo { get; set; }
    public ICommand OpenCommand { get; set; }
    public ICommand ShowCommand { get; set; }
    public ICommand DeleteCommand { get; set; }
    public void Open()
    { Process.Start("explorer", FileInfo.FullName); }
    public void Show()
    { Process.Start("explorer", FileInfo.Directory.FullName); }
    public void Delete()
    {
        FileInfo.Delete();
        RaiseDeleteEvent();
    }
}
public class FileEventArgs(File file)
{
    public File File { get; } = file;
}
