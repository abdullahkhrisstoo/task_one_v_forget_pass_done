using System;
namespace task_one_v2.App_Core.extension
{
    public static class StringExtensions
    {
        private const int Key = 5; 

        public static string Encrypt(this string input)
        {
            char[] chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)(chars[i] ^ Key);
            }
            return new string(chars);
        }

        public static string Decrypt(this string input)
        {
            char[] chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)(chars[i] ^ Key);
            }
            return new string(chars);
        }
    }
}
