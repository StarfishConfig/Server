namespace Nerosoft.Starfish.Toolkit;

/// <summary>
/// The DateTime helper class.
/// </summary>
public class DateTimeHelper
{
    /// <summary>
    /// Gets the <see cref="DateTime"/> from the given Unix time.
    /// </summary>
    /// <param name="unixTime"></param>
    /// <returns></returns>
    public static DateTime GetDateTimeFromUnixTime(long unixTime)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime);
    }

    /// <summary>
    /// Gets the Unix time from the given <see cref="DateTime"/>.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long GetUnixTimeFromDateTime(DateTime dateTime)
    {
        return (long)dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }
}
