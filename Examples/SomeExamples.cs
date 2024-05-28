using CommunityToolkit.HighPerformance.Helpers;
using Faster.Map;
using Faster.Map.DenseMap;
using static LanguageExt.Prelude;

namespace SilkPlayground.Examples;

public static class SomeExamples
{
    public static void Example()
    {
        var optional = Some(123);

        var x = optional.Match(
            Some: v => v * 2,
            None: () => 0
        );

        var testSq = Seq(1, 2, 3);

        var parityOptional = testSq
            .Where(number => number % 2 == 0)
            .HeadOrNone();

        var message = parityOptional.Match(
            Some: i => $"Found number {i}",
            None: () => "No number"
        );

        Console.WriteLine(message);
    }

    public static void PatternMatchingExample(IPattern pattern)
    {
        var t = pattern switch
        {
            PatternOne patternOne => PatternOneF(patternOne),
            PatternTwo patternTwo => PatternTwoF(),
            _ => throw new ArgumentOutOfRangeException(nameof(pattern))
        };
    }

    public static void UsingSimdLinq()
    {
        var array = Enumerable.Range(1, 1_000_000).ToArray();
        var sum = array.Sum(); // uses SimdLinqExtensions.Sum
        Console.WriteLine(sum);
    }

    public static void UsingFastHashMap()
    {
        var mapTest = new DenseMap<uint, uint>();
        mapTest.Emplace(1, 50);
        mapTest.Remove(1);
        mapTest.Get(1, out var result);
        mapTest.Update(1, 51);
        mapTest.AddOrUpdate(1, 50);
        
        Console.WriteLine(mapTest.Count);
    }

    private static bool PatternOneF(in PatternOne patternOne)
    {
        return true;
    }

    private static bool PatternTwoF()
    {
        return true;
    }

    public static void TestRefStruct()
    {
        var t = new PassedStruct
        {
            Expiration = 1,
            Inputs = [1, 2, 3, 4, 5],
            Outputs = [10, 9, 8, 7, 6]
        };
        
        TestRefStructPassing(in t);
    }

    private static void TestRefStructPassing(ref readonly PassedStruct passedStruct)
    {
        // this copies and I don't want that
        // Parallel.ForEach(passedStruct.Inputs.ToArray(), input =>
        // {
        //
        // });

        var inputs = passedStruct.Inputs;
        
        // Parallel.For(0, inputs.Length, index =>
        // {
        //     Console.WriteLine(inputs[index]); // just because it's used in a lambda the compiler can't prove it doesn't escape the stack which sucks
        //                                      // Just wanted to operate on an int array on the stack in parallel. So either I use unsafe to get a pointer 
        //                                      // to that stack allocated array (which defeats the purpose of staying in C# if I want to do these kind of 
        //                                      // micro optimizations all the time) or use ReadOnlyMemory<T> which will be on the heap and reuse that memory,
        //                                      // but for exactly this case which it's just an overhead. Or don't use Parallel class because all functions take a delegate(lambda)
        // });
        // !!!!! Use ParallelHelper from the community toolkit high-performance lib (it doesn't use delegates)
        
        // ParallelHelper.ForEach(inputs, new ItemsMultiplier(3)); // same issue, only functions that work on Memory<T> and ReadOnlyMemory<T>
        
        foreach (var input in passedStruct.Inputs)
        {
            Console.WriteLine(input);
        }
        
        Console.WriteLine($"Expiration {passedStruct.Expiration} . Inputs: {string.Join(passedStruct.Inputs[..1].ToString(), ",")}");
    }
    
    
}