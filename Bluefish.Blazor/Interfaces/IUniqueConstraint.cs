namespace Bluefish.Blazor.Interfaces;

public interface IUniqueConstraint
{
    string UniqueValue { get; }
    string[] ExistingValues { get; set; }
}
