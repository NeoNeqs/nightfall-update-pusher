using System.Security.Cryptography;

namespace Nightfall.UpdatePusher;

public static class HashAlgorithmExtensions
{
    public static string ComputeHashS(this HashAlgorithm hashAlgorithm, byte[] bytes)
    {
        byte[] fileHash = hashAlgorithm.ComputeHash(bytes);
        return BitConverter.ToString(fileHash).Replace("-", string.Empty).ToLower();
    }
}