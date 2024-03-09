using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psl.Chase.Utils
{
    public class DataConverter
    {
        #region Constants
        private const int DAY_SIZE = 5;

        private const int MONTH_SIZE = 5;

        private const int YEAR_SIZE = 11;

        private const int HOURS_SIZE = 5;

        private const int MINS_SIZE = 6;
        #endregion

        #region Public Methods
        /// <summary>
        /// Convert binary string to hex string
        /// </summary>
        /// <param name="strBinary"></param>
        /// <returns></returns>
        public static string ConvertBinaryStringToHexString(string strBinary)
        {
            StringBuilder strHex = new StringBuilder();
            string tmp = strBinary;

            while (tmp.Length > 0)
            {
                int index = tmp.Length > 4 ? (tmp.Length - 4) : 0;
                int length = tmp.Length > 4 ? 4 : tmp.Length;

                string str = tmp.Substring(index, length);
                tmp = tmp.Remove(index, length);

                UInt64 dec = ConvertBinaryStringToDecimal(str);

                string s = Convert.ToString((long)dec, 16);

                strHex = strHex.Insert(0, s);
            }

            return strHex.ToString().ToUpper();
        }

        /// <summary>
        /// Convert binary string to decimail
        /// </summary>
        /// <param name="strBinary"></param>
        /// <returns></returns>
        public static UInt64 ConvertBinaryStringToDecimal(string strBinary)
        {
            UInt64 dec = 0;

            if (strBinary.Length > 64)
            {
                throw new Exception("String is longer than 64 bits, less than 64 bits is required");
            }

            for (int i = strBinary.Length; i > 0; i--)
            {
                if (strBinary[i - 1] != '1' && strBinary[i - 1] != '0')
                    throw new Exception("String is not in binary string format");

                UInt64 temp = (UInt64)((strBinary[i - 1] == '1') ? 1 : 0);

                dec += temp << (strBinary.Length - i);
            }

            return dec;
        }

        /// <summary>
        /// convert string to byte array
        /// </summary>
        /// <param name="str">string to be converted, string</param>
        /// <returns>byte array</returns>
        public static byte[] ConvertHexStringToByteArray(string str)
        {
            float fLen = str.Length;
            int nSize = (int)Math.Ceiling(fLen / 2);

            string strArray = null;
            byte[] bytes = new byte[nSize];

            //Keep the string oven length.
            if (nSize * 2 > fLen)
            {
                strArray = "0" + str;
            }
            else
                strArray = str;

            for (int i = 0; i < nSize; i++)
            {
                int index = i * 2;
                char[] cArr = new char[] { strArray[index], strArray[index + 1] };

                string s = new string(cArr);

                try
                {
                    bytes[i] = Convert.ToByte(s, 16);
                }
                catch (System.OverflowException)
                {
                    System.Console.WriteLine(
                        "Conversion from string to byte overflowed.");
                }
                catch (System.FormatException)
                {
                    System.Console.WriteLine(
                        "The string is not formatted as a byte.");
                }
                catch (System.ArgumentNullException)
                {
                    System.Console.WriteLine(
                        "The string is null.");
                }

            }

            return bytes;
        }

        /// <summary>
        /// Converts the binary string to bool value.
        /// </summary>
        /// <param name="binBool">The bin bool.</param>
        /// <returns></returns>
        public static bool ConvertBinaryStringToBoolValue(string binBool)
        {
            bool retValue = false;
            if (!string.IsNullOrEmpty(binBool) &&
                binBool == "1")
            {
                retValue = true;
            }
            return retValue;
        }

        public static bool ConvertBinaryStringToBoolValue(string binBool, bool defaultValue)
        {
            bool retValue = defaultValue;
            if (!string.IsNullOrEmpty(binBool) &&
                binBool == "1")
            {
                retValue = true;
            }
            return retValue;
        }

        /// <summary>
        /// Converts the binary string to ASCII seven.
        /// </summary>
        /// <param name="binary">The binary.</param>
        /// <returns></returns>
        public static string ConvertBinaryStringToAsciiSeven(string binary)
        {
            string asciiseven;
            StringBuilder buffer = new StringBuilder("");
            int len = binary.Length;
            for (int i = 0; i < len; i += 7)
            {
                int j = Psl.Chase.Utils.TypeConverter.ToInt32(ConvertBinaryStringToDecimal(PadBinary(binary.Substring(i, 7), 8)).ToString());
                buffer.Append((char)j);
            }
            asciiseven = buffer.ToString();
            return asciiseven;
        }

        /// <summary>
        /// Pads the binary.
        /// </summary>
        /// <param name="binary">The binary.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static string PadBinary(string binary, int length)
        {
            string paddedBinary = string.Empty;
            int l = binary.Length;
            int pad = (length - (l % length)) % length;
            StringBuilder buffer = new StringBuilder("");
            for (int i = 0; i < pad; i++)
                buffer.Append("0");
            buffer.Append(binary);
            paddedBinary = buffer.ToString();
            return paddedBinary;
        }

        ///// <summary>
        ///// Converts the binary string to decimal.
        ///// </summary>
        ///// <param name="binary">The binary.</param>
        ///// <returns></returns>
        //public static string ConvertBinaryStringToDecimal(string binary)
        //{
        //    ulong decimalValue = 0;
        //    try
        //    {
        //        decimalValue = Convert.ToUInt64(binary, 2);
        //    }
        //    catch (OverflowException ex)
        //    {
        //        Psl.Chase.Utils.LogManager.Logger.LogError("Numeric Overflow occurred." + ex.ToString());
        //    }
        //    catch { }
        //    return decimalValue.ToString();
        //}

        /// <summary>
        /// Converts the binary string to integer.
        /// </summary>
        /// <param name="binaryString">The binary string.</param>
        /// <returns></returns>
        public static int ConvertBinaryStringToInteger(string binaryString, int defaultValue)
        {
            int integerValue = defaultValue;
            try
            {
                integerValue = Convert.ToInt32(binaryString, 2);
            }
            catch 
            {
                integerValue = defaultValue;
            }
            return integerValue;
        }

        public static long ConvertBinaryStringToLong(string binaryString)
        {
            long integerValue = 0;
            try
            {
                integerValue = Convert.ToInt64(binaryString, 2);
            }
            catch { }
            return integerValue;
        }

        public static long ConvertBinaryStringToLong(string binaryString, long defaultValue)
        {
            long integerValue = defaultValue;
            try
            {
                integerValue = Convert.ToInt64(binaryString, 2);
            }
            catch 
            {
                integerValue = defaultValue;
            }
            return integerValue;
        }

        public static string ConvertDateTimeToBinaryString(DateTime dateTime)
        {
            string binaryString = string.Empty;

            string dayString = Psl.Chase.Utils.DataConverter.ConvertIntegerValueToBinaryString(dateTime.Day);
            dayString = dayString.PadLeft(DAY_SIZE, '0');

            string monthString = Psl.Chase.Utils.DataConverter.ConvertIntegerValueToBinaryString(dateTime.Month);
            monthString = monthString.PadLeft(MONTH_SIZE, '0');

            string yearString = Psl.Chase.Utils.DataConverter.ConvertIntegerValueToBinaryString(dateTime.Year);
            yearString = yearString.PadLeft(YEAR_SIZE, '0');

            string hourString = Psl.Chase.Utils.DataConverter.ConvertIntegerValueToBinaryString(dateTime.Hour);
            hourString = hourString.PadLeft(HOURS_SIZE, '0');

            string minString = Psl.Chase.Utils.DataConverter.ConvertIntegerValueToBinaryString(dateTime.Minute);
            minString = minString.PadLeft(MINS_SIZE, '0');

            binaryString = dayString + monthString + yearString + hourString + minString;

            return binaryString;
        }

        /// <summary>
        /// Converts the binary string to date time.
        /// </summary>
        /// <param name="binaryString">The binary string.</param>
        /// <returns></returns>
        public static DateTime ConvertBinaryStringToDateTime(string binaryString)
        {
            DateTime dateTimeValue = DateTime.MinValue;
            try
            {
                binaryString = binaryString.PadLeft(32, '0');
                string dayStr = binaryString.Substring(0, DAY_SIZE);
                int day = Convert.ToInt32(dayStr, 2);

                string monthStr = binaryString.Substring(5, MONTH_SIZE);
                int month = Convert.ToInt32(monthStr, 2);

                string yearStr = binaryString.Substring(10, YEAR_SIZE);
                int year = Convert.ToInt32(yearStr, 2);

                string hourStr = binaryString.Substring(21, HOURS_SIZE);
                int hour = Convert.ToInt32(hourStr, 2);

                string minuteStr = binaryString.Substring(26, MINS_SIZE);
                int minute = Convert.ToInt32(minuteStr, 2);

                if (day > 31 || day == 0)
                    day = 31;
                if (month > 12 || month == 0)
                    month = 12;
                if (year > 2048 || year == 0)
                    year = 2048;
                if (hour > 24)
                    hour = 0;
                if (minute > 60)
                    minute = 0;
                dateTimeValue = new DateTime(year, month, day, hour, minute, 0);
            }
            catch { }
            return dateTimeValue;
        }

        public static DateTime ConvertBinaryStringToDateTime(string binaryString, DateTime defaultDatetime)
        {
            DateTime dateTimeValue = defaultDatetime;
            try
            {
                binaryString = binaryString.PadLeft(32, '0');
                string dayStr = binaryString.Substring(0, DAY_SIZE);
                int day = Convert.ToInt32(dayStr, 2);

                string monthStr = binaryString.Substring(5, MONTH_SIZE);
                int month = Convert.ToInt32(monthStr, 2);

                string yearStr = binaryString.Substring(10, YEAR_SIZE);
                int year = Convert.ToInt32(yearStr, 2);

                string hourStr = binaryString.Substring(21, HOURS_SIZE);
                int hour = Convert.ToInt32(hourStr, 2);

                string minuteStr = binaryString.Substring(26, MINS_SIZE);
                int minute = Convert.ToInt32(minuteStr, 2);

                if (day > 31 || day == 0)
                    day = 31;
                if (month > 12 || month == 0)
                    month = 12;
                if (year > 2048 || year == 0)
                    year = 2048;
                if (hour > 24)
                    hour = 0;
                if (minute > 60)
                    minute = 0;
                dateTimeValue = new DateTime(year, month, day, hour, minute, 0);
            }
            catch 
            {
                dateTimeValue = defaultDatetime;
            }
            return dateTimeValue;
        }

        /// <summary>
        /// Converts the binary string to ASCII string.
        /// </summary>
        /// <param name="binaryString">The binary string.</param>
        /// <returns></returns>
        public static string ConvertBinaryStringToASCIIString(string binaryString)
        {
            string stringValue = string.Empty;
            string hexStr = DataConverter.ConvertBinaryStringToHexString(binaryString);
            byte[] byteArray = DataConverter.ConvertHexStringToByteArray(hexStr);
            stringValue = ASCIIEncoding.ASCII.GetString(byteArray);
            return stringValue;
        }

        /// <summary>
        /// Converts the ASCII seven to binary string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ConvertAsciiSevenToBinaryString(string value)
        {
            string binary;
            StringBuilder buffer = new StringBuilder("");
            int len = value.Length;
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(value);
            for (int i = 0; i < len; i++)
            {
                buffer.Append(PadBinary(ConvertDecimalToBinaryString((bytes[i] % 128).ToString()), 8).Substring(1, 7));
            }
            binary = buffer.ToString();
            return binary;
        }

        /// <summary>
        /// Converts the decimal to binary string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ConvertDecimalToBinaryString(string value)
        {
            string binary = string.Empty;
            ulong numValue = Psl.Chase.Utils.TypeConverter.ToUInt64(value);
            binary = Psl.Chase.Utils.TypeConverter.ToBinaryString(numValue);
            return binary;
        }

        /// <summary>
        /// Converts the string to binary string.
        /// </summary>
        /// <param name="stringValue">The string value.</param>
        /// <param name="totalLength">The total length.</param>
        /// <returns></returns>
        public static string ConvertStringToBinaryString(string stringValue, int totalLength)
        {
            string binaryString = string.Empty;
            stringValue = stringValue.PadRight(totalLength, ' ');
            stringValue = stringValue.Substring(0, totalLength);
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(stringValue);
            string hexString = ConvertByteArrayToHexString(byteArray);
            binaryString = ConvertHexStringToBinaryString(hexString);
            return binaryString;
        }

        /// <summary>
        /// Converts the integer value to binary string.
        /// </summary>
        /// <param name="intValue">The int value.</param>
        /// <returns></returns>
        public static string ConvertIntegerValueToBinaryString(int intValue)
        {
            string binaryString = string.Empty;
            try
            {
                binaryString = Convert.ToString(intValue, 2);
            }
            catch { }
            return binaryString;
        }

        public static string ConvertLongValueToBinaryString(long intValue)
        {
            string binaryString = string.Empty;
            try
            {
                binaryString = Convert.ToString(intValue, 2);
            }
            catch { }
            return binaryString;
        }

        public static string ConvertBoolValueToBinaryString(bool value)
        {
            string binaryString = string.Empty;
            if (value)
            {
                binaryString = "1";
            }
            else
            {
                binaryString = "0";
            }
            return binaryString;
        }

        /// <summary>
        /// Convert HexString to binary string
        /// </summary>
        /// <param name="strHex"></param>
        /// <returns></returns>
        public static string ConvertHexStringToBinaryString(string strHex)
        {
            string binStr = string.Empty;
            int strLen = strHex.Length;

            try
            {
                for (int i = 0; i < strLen; i++)
                {
                    binStr += CharToBinaryString(strHex[i]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return binStr;
        }

        /// <summary>
        /// Convert byte array to Hex string
        /// </summary>
        /// <param name="byte_array"></param>
        /// <returns></returns>
        public static string ConvertByteArrayToHexString(byte[] byte_array)
        {
            string s = string.Empty;

            try
            {
                for (int i = 0; i < byte_array.Length; i++) s += string.Format("{0:X2}", byte_array[i]);
            }
            catch { }
            return s;
        }
        #endregion

        #region Private Methods
        private static string CharToBinaryString(char c)
        {
            switch (c)
            {
                case '0':
                    return "0000";
                case '1':
                    return "0001";
                case '2':
                    return "0010";
                case '3':
                    return "0011";
                case '4':
                    return "0100";
                case '5':
                    return "0101";
                case '6':
                    return "0110";
                case '7':
                    return "0111";
                case '8':
                    return "1000";
                case '9':
                    return "1001";
                case 'a':
                case 'A':
                    return "1010";
                case 'b':
                case 'B':
                    return "1011";
                case 'c':
                case 'C':
                    return "1100";
                case 'd':
                case 'D':
                    return "1101";
                case 'e':
                case 'E':
                    return "1110";
                case 'f':
                case 'F':
                    return "1111";
                default:
                    throw new Exception("Input is not a  Hex. string");
            }
        }
        #endregion
    }
}
