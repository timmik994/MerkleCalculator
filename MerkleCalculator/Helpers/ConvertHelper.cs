using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerkleCalculator.Helpers;

/// <summary>
/// This helper contains value conversion methods.
/// </summary>
public static class ConvertHelper
{
    /// <summary>
    /// Converts byte array containing hash to its string representation.
    /// </summary>
    /// <param name="bytes">The bytes array with hash value</param>
    /// <returns>The hash string.</returns>
    public static string ShowHashAsString(byte[] bytes)
    {
        return Convert.ToHexString(bytes);
    }

    /// <summary>
    /// Gets bytes from string.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The string bytes.</returns>
    public static byte[] GetBytesFromString(string input)
    {
        return Encoding.UTF8.GetBytes(input);
    }
}
