using System;
using System.Linq;

namespace LibAmiibo.Helper
{
    /// <summary>
    /// A utility class for working with byte arrays and hexadecimal strings.
    /// </summary>
    internal class ByteHelpers
    {
        /// <summary>
        /// Converts a hexadecimal string to a byte array.
        /// </summary>
        /// <param name="hex">The hexadecimal string to convert.</param>
        /// <returns>A byte array representing the converted hexadecimal string.</returns>
        public static byte[] StringToByteArray(string hex)
        {
            // Create a byte array by parsing the input hexadecimal string.
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
