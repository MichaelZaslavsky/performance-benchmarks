using BenchmarkDotNet.Attributes;

namespace PerformanceBenchmarks;

/// <summary>
/// <para>
/// Check: Differences between initializing a list with/without a capacity.
/// Conclusion: If the count is known, it is recommended to specify the count as it is faster and uses less memory allocation.
/// Reason: If not specifying the list's capacity, the list is initialized with the 4 elements, and each time a new element is added to the list, it first checks
///         that the array capacity has the interior reached its maximum capacity.
///         If it reaches the maximum value, it creates a new array twice the length of the previous array,
///         copies all the elements of the previous array into the new array, and then adds the new element to the array.
///         By specifying the list's capacity, we eliminate the cost of new array prototyping and copying.
/// </para>
/// <para>
/// Results:
/// |                             Method |     Mean |    Error |   StdDev |   Gen0 | Allocated |
/// |----------------------------------- |---------:|---------:|---------:|-------:|----------:|
/// |          InitWithSpecifingCapacity | 20.54 ns | 0.410 ns | 0.421 ns | 0.0153 |      96 B |
/// |       InitWithoutSpecifingCapacity | 49.39 ns | 0.987 ns | 1.098 ns | 0.0344 |     216 B |
/// |    AddNumbersWithSpecifingCapacity | 25.59 ns | 0.486 ns | 0.520 ns | 0.0153 |      96 B |
/// | AddNumbersWithoutSpecifingCapacity | 50.86 ns | 1.030 ns | 1.102 ns | 0.0344 |     216 B |
/// </para>
/// </summary>
[MemoryDiagnoser]
public class ListInitialization
{
    private const int _count = 10;

    [Benchmark]
    public void InitWithSpecifingCapacity()
    {
        List<int> _ = new(_count) { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    }

    [Benchmark]
    public void InitWithoutSpecifingCapacity()
    {
        List<int> _ = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    }

    [Benchmark]
    public void AddNumbersWithSpecifingCapacity()
    {
        List<int> numbers = new(_count);
        for (int i = 0; i < _count; i++)
        {
            numbers.Add(i);
        }
    }

    [Benchmark]
    public void AddNumbersWithoutSpecifingCapacity()
    {
        List<int> numbers = new();
        for (int i = 0; i < _count; i++)
        {
            numbers.Add(i);
        }
    }
}
