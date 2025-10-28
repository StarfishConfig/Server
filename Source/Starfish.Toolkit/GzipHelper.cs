using System.IO.Compression;

namespace Nerosoft.Starfish.Toolkit;

internal static class GzipHelper
{
    /// <summary>
    /// Compress a string and encode it to Base64
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string CompressToBase64(string source)
    {
        var buffer = Compress(source);

        return Convert.ToBase64String(buffer);
    }

    /// <summary>
    /// Compress a string to Gzip byte array
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static byte[] Compress(string source)
    {
        var data = Encoding.UTF8.GetBytes(source);

        var stream = new MemoryStream();
        var zip = new GZipStream(stream, CompressionMode.Compress, true);
        zip.Write(data, 0, data.Length);
        zip.Close();
        var buffer = new byte[stream.Length];
        stream.Position = 0;
        _ = stream.Read(buffer, 0, buffer.Length);
        stream.Close();

        return buffer;
    }

    /// <summary>
    /// Decompress from Base64 encoded Gzip string
    /// </summary>
    /// <param name="base64Data"></param>
    /// <returns></returns>
    /// <remarks>
    /// This method first decodes the Base64 string to get the compressed byte array,
    /// then decompresses the byte array using Gzip and returns the original string.
    /// </remarks>
    public static string DecompressFromBase64(string base64Data)
    {
        var data = Convert.FromBase64String(base64Data);
        return Decompress(data);
    }

    /// <summary>
    /// Decompress Gzip compressed byte array
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string Decompress(byte[] data)
    {
        return Decompress(data, data.Length);
    }

    /// <summary>
    /// Decompress Gzip compressed byte array with specified length
    /// </summary>
    /// <param name="data"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static string Decompress(byte[] data, int count)
    {
        var stream = new MemoryStream(data, 0, count);
        var zip = new GZipStream(stream, CompressionMode.Decompress, true);
        var destStream = new MemoryStream();
        var buffer = new byte[0x1000];
        while (true)
        {
            var reader = zip.Read(buffer, 0, buffer.Length);
            if (reader <= 0)
            {
                break;
            }

            destStream.Write(buffer, 0, reader);
        }

        zip.Close();
        stream.Close();
        destStream.Position = 0;
        buffer = destStream.ToArray();
        destStream.Close();
        return Encoding.UTF8.GetString(buffer);
    }
}