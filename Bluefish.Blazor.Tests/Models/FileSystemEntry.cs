namespace Bluefish.Blazor.Tests.Models;

public class FileSystemEntry
{
    public FileSystemEntry(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public override string ToString()
    {
        return Name;
    }
}
