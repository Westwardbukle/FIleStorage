using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace FileStorage.Common.Utils;

/// <summary>
/// Расширения для работы с Hash-суммами
/// </summary>
public static class HashUtils
{
    /// <summary>
    /// Вычисляет Hash на основе MD5
    /// </summary>
    /// <param name="file">Файл</param>
    /// <returns>Hash</returns>
    public static string ComputeMD5(this IFormFile file)
    {
        Stream st = file.OpenReadStream();
        MemoryStream mst = new MemoryStream();
        st.CopyTo(mst);
        return ToMD5Hash(mst.ToArray());
    }

    private static string ToMD5Hash(byte[] bytes)
    {
        using (var md5 = MD5.Create())
        {
            return BitConverter.ToString(md5.ComputeHash(bytes))
                .Replace("-", string.Empty)
                .ToLower();
        }
    }
}