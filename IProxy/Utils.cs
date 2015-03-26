//The MIT License (MIT)
//
//Copyright (c) 2015 Fabian Fischer
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using IProxy.Networking;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public static class Utils
{
    public static int FromString(string x)
    {
        if (x.StartsWith("0x")) return int.Parse(x.Substring(2), NumberStyles.HexNumber);
        return int.Parse(x);
    }

    /// <summary>
    ///     Indicates whether a specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="value">value: The string to test.</param>
    /// <returns>
    ///     true if the value parameter is null or System.String.Empty, or if value consists exclusively of white-space
    ///     characters.
    /// </returns>
    public static bool IsNullOrWhiteSpace(this string value)
    {
        if (value == null)
        {
            return true;
        }
        int index = 0;
        while (index < value.Length)
        {
            if (char.IsWhiteSpace(value[index]))
            {
                index++;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public static string To4Hex(short x)
    {
        return "0x" + x.ToString("x4");
    }

    public static string To2Hex(short x)
    {
        return "0x" + x.ToString("x2");
    }

    public static string GetCommaSepString<T>(T[] arr)
    {
        StringBuilder ret = new StringBuilder();
        for (int i = 0; i < arr.Length; i++)
        {
            if (i != 0) ret.Append(", ");
            ret.Append(arr[i]);
        }
        return ret.ToString();
    }

    public static IEnumerable<int> StringListToIntList(IEnumerable<string> strList)
    {
        List<int> ret = new List<int>();
        foreach (string i in strList)
        {
            try
            {
                ret.Add(FromString(i));
            }
            catch
            {
            }
        }
        return ret;
    }

    public static int[] FromCommaSepString32(string x)
    {
        if (IsNullOrWhiteSpace(x)) return new int[0];
        return x.Split(',').Select(_ => FromString(_.Trim())).ToArray();
    }

    public static short[] FromCommaSepString16(string x)
    {
        if (IsNullOrWhiteSpace(x)) return new short[0];
        return x.Split(',').Select(_ => (short)FromString(_.Trim())).ToArray();
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        if (list == null) return;
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box); while (!(box[0] < n * (uint.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static string ToSafeText(this string str)
    {
        return Encoding.ASCII.GetString(
            Encoding.Convert(
                Encoding.UTF8,
                Encoding.GetEncoding(
                    Encoding.ASCII.EncodingName,
                    new EncoderReplacementFallback(string.Empty),
                    new DecoderExceptionFallback()
                    ),
                Encoding.UTF8.GetBytes(str)
                )
            );
    }

    public static T GetEnumByName<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static string GetEnumName<T>(object value)
    {
        return Enum.GetName(typeof(T), value);
    }

    public static T Exec<T>(this Task<T> task)
    {
        task.Wait();
        return task.Result;
    }

    public static byte[] SHA1(string val)
    {
        SHA1Managed sha1 = new SHA1Managed();
        return sha1.ComputeHash(Encoding.UTF8.GetBytes(val));
    }

    /// <summary>
    /// Converts a bool to an Int32.
    /// </summary>
    /// <param name="value">The Boolean to check</param>
    /// <returns>1 if the bool is true.\n0 if the bool is false.</returns>
    public static int ToInt32(this bool value)
    {
        return value ? 1 : 0;
    }

    public static string ToCommaSepString<T>(this T[] arr)
    {
        StringBuilder ret = new StringBuilder();
        for (var i = 0; i < arr.Length; i++)
        {
            if (i != 0) ret.Append(", ");
            ret.Append(arr[i].ToString());
        }
        return ret.ToString();
    }

    public static T[] CommaToArray<T>(this string x)
    {
        if (typeof(T) == typeof(ushort))
            return x.Split(',').Select(_ => (T)(object)(ushort)FromString(_.Trim())).ToArray();
        if (typeof(T) == typeof(short))
            return x.Split(',').Select(_ => (T)(object)(short)FromString(_.Trim())).ToArray();
        else  //assume int
            return x.Split(',').Select(_ => (T)(object)FromString(_.Trim())).ToArray();
    }

    public static int ToUnixTimestamp(this DateTime dateTime)
    {
        return (int)(dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
    }

    public static object To4Hex(ushort x)
    {
        return "0x" + x.ToString("x4");
    }

    public static bool ContainsIgnoreCase(this string self, string val)
    {
        return self.IndexOf(val, StringComparison.InvariantCultureIgnoreCase) != -1;
    }
    public static bool EqualsIgnoreCase(this string self, string val)
    {
        return self.Equals(val, StringComparison.InvariantCultureIgnoreCase);
    }

    public static T ChangePacketType<T>(Packet pkt) where T : Packet
    {
        return pkt as T;
    }
}
