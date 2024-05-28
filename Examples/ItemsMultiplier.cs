using CommunityToolkit.HighPerformance.Helpers;

namespace SilkPlayground.Examples;

public readonly struct ItemsMultiplier(int factor) : IRefAction<int>
{
    public void Invoke(ref int item) => item *= factor;
}