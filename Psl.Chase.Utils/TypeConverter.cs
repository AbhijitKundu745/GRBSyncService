using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psl.Chase.Utils
{
    /// <summary>
    /// TypeConverter
    /// </summary>
    public static class TypeConverter
    {
        #region Public Methods
        /// <summary>
        /// Toes the int32.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns></returns>
        public static Int32 ToInt32(string inputValue)
        {
            Int32 convertedValue = default(Int32);
            try
            {
                convertedValue = Int32.Parse(inputValue);
            }
            catch
            { }
            return convertedValue;
        }

        /// <summary>
        /// Toes the U int64.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns></returns>
        public static UInt64 ToUInt64(string inputValue)
        {
            UInt64 convertedValue = default(UInt64);
            try
            {
                convertedValue = UInt64.Parse(inputValue);
            }
            catch
            { }
            return convertedValue;
        }

        /// <summary>
        /// Toes the binary string.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns></returns>
        public static string ToBinaryString(ulong inputValue)
        {
            string binaryString = string.Empty;
            System.Collections.BitArray bitArray = new System.Collections.BitArray(BitConverter.GetBytes(inputValue));
            for (int i = bitArray.Count - 1; i >= 0; i--)
            {
                bool bit = bitArray[i];
                binaryString = binaryString + ((bit) ? "1" : "0");
            }
            binaryString = binaryString.TrimStart('0');
            return binaryString;
        }
        #endregion
    }
}
