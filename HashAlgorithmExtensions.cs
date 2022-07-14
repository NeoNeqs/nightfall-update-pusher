using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Nightfall.UpdatePusher;

public static class HashAlgorithmExtensions
{
    public static string ComputeHashS(this HashAlgorithm hashAlgorithm, byte[] bytes)
    {
        byte[] fileHash = hashAlgorithm.ComputeHash(bytes);
        return BitConverter.ToString(fileHash).Replace("-", string.Empty).ToLower();
    }

    public static string? ComputeHashFileS(this HashAlgorithm hashAlgorithm, string filePath)
    {
        byte[] bytes;

        try
        {
            bytes = File.ReadAllBytes(filePath);
        }
        catch (Exception e)
        {
            Utils.LogError(e);
            return null;
        }

        return ComputeHashS(hashAlgorithm, bytes);
    }
}