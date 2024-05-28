namespace SilkPlayground.Examples;

public readonly record struct PatternOne() : IPattern
{
    public string Name { get; init; } = string.Empty;
}