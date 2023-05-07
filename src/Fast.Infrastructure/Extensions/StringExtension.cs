using System.Security.Cryptography;
using System.Text;

namespace Fast.Infrastructure.Extensions;

public static class StringExtension
{
    public static bool IsNullOrWhiteSpace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static string ToMd5(this string str)
    {
        var inputBytes = Encoding.UTF8.GetBytes(str);
        var hashBytes = MD5.HashData(inputBytes);

        var sb = new StringBuilder();
        foreach (var hashByte in hashBytes) sb.Append(hashByte.ToString("X2"));

        return sb.ToString();
    }
}