using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LogDashboard.Extensions
{
    public static class StringArrayExtensions
    {
        public static string TryGetValue(this string[] array, int index)
        {
            try
            {
                return array[index].Trim().Replace(',', '.');
            }
            catch
            {
                return null;
            }
        }

        public static string GetMD5(this string str)
        {
            using (var md5 = MD5.Create())
            {
                try
                {
                    var strResult = md5.ComputeHash(Encoding.ASCII.GetBytes(str));
                    return BitConverter.ToString(strResult).Replace("-", "");
                }
                catch (Exception e)
                {
                    return "";
                }

            }
        }
    }
}
