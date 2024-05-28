namespace SilkPlayground.Examples;

public readonly ref struct PassedStruct
{
    public int Expiration { get; init; }
    public ReadOnlySpan<int> Inputs { get; init; }
    public ReadOnlySpan<int> Outputs { get; init; }
}