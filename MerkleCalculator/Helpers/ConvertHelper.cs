using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerkleCalculator.Helpers;

public static class ConvertHelper
{
    public static string ToHexString(byte[] bytes)
    {
        return Convert.ToHexString(bytes);
    }

    public static byte[] GetBytesFromString(string input)
    {
        return Encoding.UTF8.GetBytes(input);
    }
}
