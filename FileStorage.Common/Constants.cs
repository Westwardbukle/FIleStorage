namespace FileStorage.Common;

/// <summary>
/// Константные значения
/// </summary>
public static class Constants
{
    /// <summary>
    /// Ограничение тела POST-запроса в 50 МБ
    /// </summary>
    public const long MultipartBodyLengthLimit50MB = 52428800;

    /// <summary>
    /// Словарь для валидации файла по сигнатуре
    /// </summary>
    public static readonly Dictionary<string, List<byte[]>> FileSignature = new()
    {
        {
            ".jpeg", new List<byte[]>
            {
                new byte[] {0xFF, 0xD8, 0xFF}
            }
        },
        {
            ".jpg", new List<byte[]>
            {
                new byte[] {0xFF, 0xD8, 0xFF}
            }
        },
        {
            ".doc", new List<byte[]>
            {
                new byte[] {0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1}
            }
        },
        {
            ".docx", new List<byte[]>
            {
                new byte[] {0x50, 0x4B, 0x03, 0x04}
            }
        },
        {
            ".xls", new List<byte[]>
            {
                new byte[] {0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1}
            }
        },
        {
            ".xlsx", new List<byte[]>
            {
                new byte[] {0x50, 0x4B, 0x03, 0x04}
            }
        },
        {
            ".mp3", new List<byte[]>
            {
                new byte[] {0x49, 0x44, 0x33}
            }
        },
        {
            ".gif", new List<byte[]>
            {
                new byte[] {0x47, 0x49, 0x46, 0x38}
            }
        },
        {
            ".png", new List<byte[]>
            {
                new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A}
            }
        },
        {
            ".pdf", new List<byte[]>
            {
                new byte[] {0x25, 0x50, 0x44, 0x46, 0x2D}
            }
        },
        {
            ".odt", new List<byte[]>
            {
                new byte[] {0x50, 0x4B, 0x03, 0x04}
            }
        },
        {
            ".ods", new List<byte[]>
            {
                new byte[] {0x50, 0x4B, 0x03, 0x04}
            }
        },
        {
            ".apk", new List<byte[]>
            {
                new byte[] {0x50, 0x4B, 0x03, 0x04}
            }
        }
    };
}

