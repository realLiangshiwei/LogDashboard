using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LogDashboard.Extensions
{

    internal static class MD5Extensions
    {
        public static string ToMD5(this string input, MD5Length length = MD5Length.L32)
        {
            return MD5Encrypt.MD5(input, length);
        }
    }

    internal class MD5Encrypt
    {
        public static string MD5(string srcString, MD5Length length = MD5Length.L32)
        {
            using (MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] bytes_md5_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_md5_out = md5.ComputeHash(bytes_md5_in);

                string str_md5_out = length == MD5Length.L32
                    ? BitConverter.ToString(bytes_md5_out)
                    : BitConverter.ToString(bytes_md5_out, 4, 8);

                str_md5_out = str_md5_out.Replace("-", "");
                return str_md5_out;
            }
        }
    }

    internal enum MD5Length
    {
        L16 = 16,
        L32 = 32
    }
}
