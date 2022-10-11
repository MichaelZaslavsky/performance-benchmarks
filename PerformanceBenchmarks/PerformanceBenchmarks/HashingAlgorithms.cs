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
/// |             Method |               Mean |            Error |            StdDev |      Gen0 |      Gen1 |      Gen2 |    Allocated |
/// |------------------- |-------------------:|-----------------:|------------------:|----------:|----------:|----------:|-------------:|
/// |         ComputeMD5 |         1,410.5 ns |         50.12 ns |         147.01 ns |    0.0324 |         - |         - |        208 B |
/// |      ComputeSHA256 |         1,164.5 ns |         23.14 ns |          32.44 ns |    0.0381 |         - |         - |        240 B |
/// |      ComputeSHA384 |         1,936.3 ns |         28.42 ns |          26.59 ns |    0.0420 |         - |         - |        272 B |
/// |      ComputeSHA512 |         1,988.6 ns |         39.75 ns |          61.89 ns |    0.0458 |         - |         - |        304 B |
/// |      ComputeRIPEMD |           840.7 ns |          8.41 ns |           7.03 ns |    0.0553 |         - |         - |        352 B |
/// |      ComputeArgon2 | 2,651,472,193.9 ns | 60,040,756.62 ns | 176,089,145.71 ns | 1000.0000 | 1000.0000 | 1000.0000 | 1078225008 B |
/// |      ComputeBcrypt |   266,580,587.5 ns |  3,580,869.91 ns |   2,795,707.68 ns |         - |         - |         - |       6580 B |
/// | ComputeHMACBlake2B |         2,681.1 ns |         53.43 ns |          90.72 ns |    0.1640 |         - |         - |       1048 B |
/// |     ComputeHMACMD5 |         3,087.8 ns |         58.98 ns |          49.25 ns |    0.0877 |    0.0153 |    0.0038 |        552 B |
/// |    ComputeHMACSHA1 |         3,428.6 ns |         66.04 ns |          90.39 ns |    0.0877 |    0.0114 |         - |        568 B |
/// |  ComputeHMACSHA256 |         2,733.9 ns |         53.97 ns |          88.67 ns |    0.0916 |    0.0191 |         - |        584 B |
/// |  ComputeHMACSHA384 |         5,800.5 ns |        113.13 ns |         185.88 ns |    0.1221 |    0.0076 |         - |        808 B |
/// |  ComputeHMACSHA512 |         5,901.9 ns |        114.35 ns |         164.00 ns |    0.1297 |    0.0076 |         - |        840 B |
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
        instance.MemorySize = 1024 * 1024;

        instance.GetBytes(16);
    }

    [Benchmark]
    public void ComputeBcrypt()
    {
        BCrypt.Net.BCrypt.HashPassword("Some Text");
    }


    [Benchmark]
    public void ComputeHMACBlake2B()
    {
        HMACBlake2B instance = new(512);
        instance.ComputeHash(inputBytes);
    }

    [Benchmark]
    public void ComputeHMACMD5()
    {
        HMACMD5 instance = new();
        instance.ComputeHash(inputBytes);
    }

    [Benchmark]
    public void ComputeHMACSHA1()
    {
        HMACSHA1 instance = new();
        instance.ComputeHash(inputBytes);
    }

    [Benchmark]
    public void ComputeHMACSHA256()
    {
        HMACSHA256 instance = new();
        instance.ComputeHash(inputBytes);
    }

    [Benchmark]
    public void ComputeHMACSHA384()
    {
        HMACSHA384 instance = new();
        instance.ComputeHash(inputBytes);
    }

    [Benchmark]
    public void ComputeHMACSHA512()
    {
        HMACSHA512 instance = new();
        instance.ComputeHash(inputBytes);
    }
}
