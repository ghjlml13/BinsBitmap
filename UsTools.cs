using System;
using System.Collections.Generic;
using System.Text;

namespace Arinc708
{
    public static class UsTools
    {
        public static List<int> GetListFromString(string source, int min, int max)
        {
            List<int> il = new List<int> ();

            string[] indexes = source.Split(new Char[] { ' ', ',', '.', ':' });
            int i =0;
            foreach (string s in indexes)
            {
                try
                {
                    i = int.Parse(s);
                }
                catch
                {
                    i = 0;
                }
                finally
                {
                    if (i >= min && i <= max)
                    {
                        il.Add(i);
                    }
                }
            }
            return il;
        }

        public static string Reverse(string s)
        {
            int len = s.Length;
            if (len > 1)
                return Reverse(s.Substring(1, len - 1)) + s[0].ToString();
            else
                return s;
        }

        /// <summary>
        /// i.e : GetBinaryRepresentation(11, 4, true) -> '1011'
        /// </summary>
        /// <param taskName="x"></param>
        /// <param taskName="d"></param>
        /// <param taskName="WindosCalculatorStyle"></param>
        /// <returns>A binary string representation</returns>
        public static string GetBinaryRepresentation(UInt64 x, int d, bool WindosCalculatorStyle)
        {
            StringBuilder sb = new StringBuilder();
            while (d > 0)
            {
                if ((x & 1) > 0) sb.Append('1');
                else sb.Append('0');
                x >>= 1;
                d--;
            }
            return (WindosCalculatorStyle ? Reverse(sb.ToString()) : sb.ToString());
        }

        public static string IntList2CommaSeparetedString(List<int> il)
        {
            StringBuilder sb = new StringBuilder();

            if (il.Count > 0)
            {
                foreach (int ii in il)
                {
                    sb.Append(ii.ToString() + ",");
                }
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

        public static void StringToByte(out byte[] b, string str)
        {
            if (str != null)
            {
                b = new byte[str.Length];
                for (int x = 0; x < str.Length; ++x)
                {
                    b[x] = (byte)str[x];
                }
            }
            else
            {
                throw new ArgumentException("The input string must be not null", "str");
            }
        }

        public static int HexByteValue(byte what)
        {
            short val = Convert.ToInt16(what);
            if (val > 47 && val < 58)
            {
                return val - 48;
            }
            else if ((val > 64 && val < 71) || (val > 96 && val < 103))
            {
                return val - 55;
            }
            throw new ArgumentException("The byte is not a valid hex nibble rep", "what");
        }
    }
}
