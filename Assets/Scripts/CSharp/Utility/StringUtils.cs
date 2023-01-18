using System.Security.Cryptography;
using System.Text;
using System;

public class StringUtils 
{
    public static string MD5(string str)
    {
        byte[] resultBytes = Encoding.UTF8.GetBytes(str);
        MD5 md5 = new MD5CryptoServiceProvider();

        byte[] outPut = md5.ComputeHash(resultBytes);
        StringBuilder hashString = new StringBuilder();

        for (int i = 0; i < outPut.Length; i++)
        {
            hashString.Append(Convert.ToString(outPut[i], 16).PadLeft(2, '0'));
        }
        return hashString.ToString();
    }
}