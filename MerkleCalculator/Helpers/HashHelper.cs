using System.Security.Cryptography;
using System.Text;

namespace MerkleCalculator.Helpers;

/// <summary>
/// Helper that contains functions to work with string hash we need.
/// </summary>
public static class HashHelper
{
    /// <summary>
    /// Calculates hash by formula:
    /// SHA256(SHA256(tag)+SHA256(tag)+input)
    /// </summary>
    /// <param name="input">The input string we want to hash</param>
    /// <param name="tag">The hash tag we will use.</param>
    /// <returns>The hash result.</returns>
    public static byte[] CalculateTaggedHash(string input, string tag)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        return CalculateTaggedHash(inputBytes, tag);
    }

    public static byte[] CalculateTaggedHash(byte[] input, string tag) 
    {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] tagBytes = Encoding.UTF8.GetBytes(tag);

        byte[] tagHash = sha256Hash.ComputeHash(tagBytes);

        byte[] sequenceToHash = tagHash.Concat(tagHash).Concat(input).ToArray();

        return sha256Hash.ComputeHash(sequenceToHash);
    }
}
