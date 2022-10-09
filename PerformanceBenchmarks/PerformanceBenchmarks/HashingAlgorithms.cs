using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace PerformanceBenchmarks;

/// <summary>
/// <para>
/// Check: Differences between compute hashing algorithms.
/// Conclusion: The faster algorithms are usually the less secure ones. Usually, it will be better to choose the more secure hashing algorithms.
///             It's recommended to use the SHA-2 family (SHA-256/SHA-512) hashing algorithms as they are much more secure.
///             SHA-256 is computed with 32-bit words and SHA-512 with 64-bit words.
/// Reason: The less secured algorithms are faster because they do fewer hashing manipulations on the input.
/// </para>
/// <para>
/// Results:
/// |        Method |     Mean |    Error |    StdDev |   Median |   Gen0 | Allocated |
/// |-------------- |---------:|---------:|----------:|---------:|-------:|----------:|
/// |    ComputeMD5 | 553.2 ns | 12.06 ns |  33.82 ns | 547.6 ns | 0.0324 |     208 B |
/// | ComputeSHA256 | 483.6 ns | 12.37 ns |  34.06 ns | 478.0 ns | 0.0381 |     240 B |
/// | ComputeSHA384 | 939.1 ns | 62.84 ns | 176.22 ns | 876.5 ns | 0.0429 |     272 B |
/// | ComputeSHA512 | 880.1 ns | 31.94 ns |  90.10 ns | 864.7 ns | 0.0477 |     304 B |
/// | ComputeRIPEMD | 374.5 ns | 11.80 ns |  33.85 ns | 370.8 ns | 0.0553 |     352 B |
/// </para>
/// </summary>
[MemoryDiagnoser]
public class HashingAlgorithms
{
    private byte[] inputBytes = Encoding.ASCII.GetBytes("Some Text");

    [Benchmark]
    public void ComputeMD5()
    {
        using MD5 instance = MD5.Create();
        instance.ComputeHash(inputBytes);
    }

    [Benchmark]
    public void ComputeSHA256()
    {
        using SHA256 instance = SHA256.Create();
        instance.ComputeHash(inputBytes);
    }

    [Benchmark]
    public void ComputeSHA384()
    {
        using SHA384 instance = SHA384.Create();
        instance.ComputeHash(inputBytes);
    }

    [Benchmark]
    public void ComputeSHA512()
    {
        using SHA512 instance = SHA512.Create();
        instance.ComputeHash(inputBytes);
    }

    [Benchmark]
    public void ComputeRIPEMD()
    {
        SshNet.Security.Cryptography.RIPEMD160 instance = new();
        instance.ComputeHash(inputBytes);
    }
}
