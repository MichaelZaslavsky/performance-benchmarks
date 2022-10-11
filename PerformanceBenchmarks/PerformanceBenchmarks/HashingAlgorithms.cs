using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Konscious.Security.Cryptography;

namespace PerformanceBenchmarks;

/// <summary>
/// <para>
/// Check: Differences between compute hashing algorithms.
/// Conclusion: The faster algorithms are usually the less secure ones. Usually, it will be better to choose the more secure hashing algorithms.
///             It's recommended to use the SHA-2 family (SHA-256/SHA-512) hashing algorithms as they are much more secure.
///             SHA-256 is computed with 32-bit words and SHA-512 with 64-bit words.
///             A very secure and slow hashing algorithm is Argon2. It is usually used for strong passwords. This algorithm also takes lots of memory.
/// Reason: The less secured algorithms are faster because they do fewer hashing manipulations on the input.
/// </para>
/// <para>
/// Results:
/// |        Method |           Mean |         Error |        StdDev |     Gen0 |     Gen1 |     Gen2 | Allocated |
/// |-------------- |---------------:|--------------:|--------------:|---------:|---------:|---------:|----------:|
/// |    ComputeMD5 |     1,316.6 ns |      35.17 ns |     102.02 ns |   0.0324 |        - |        - |     208 B |
/// | ComputeSHA256 |     1,137.3 ns |      22.69 ns |      51.21 ns |   0.0381 |        - |        - |     240 B |
/// | ComputeSHA384 |     1,919.9 ns |      36.90 ns |      77.84 ns |   0.0420 |        - |        - |     272 B |
/// | ComputeSHA512 |     1,938.6 ns |      38.35 ns |      81.73 ns |   0.0458 |        - |        - |     304 B |
/// | ComputeRIPEMD |       844.9 ns |      16.80 ns |      39.28 ns |   0.0553 |        - |        - |     352 B |
/// | ComputeArgon2 | 4,043,792.4 ns | 104,792.79 ns | 308,983.90 ns | 332.0313 | 332.0313 | 332.0313 | 1334275 B |
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

    [Benchmark]
    public void ComputeArgon2()
    {
        Argon2id instance = new(inputBytes);

        instance.Salt = Encoding.ASCII.GetBytes("Some Salt");
        instance.DegreeOfParallelism = 4;
        instance.Iterations = 2;
        instance.MemorySize = 1024;

        instance.GetBytes(16);
    }
}
