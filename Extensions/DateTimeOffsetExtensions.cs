namespace ObjectMapper.Extensions
{
    internal static class DateTimeOffsetExtensions
    {
        // Source: https://learn.microsoft.com/en-us/dotnet/standard/datetime/converting-between-datetime-and-offset#a-general-purpose-conversion-method
        public static object ToDateTime(this DateTimeOffset dateTime)
        {
            if (dateTime.Offset.Equals(TimeSpan.Zero))
                return dateTime.UtcDateTime;
            else if (dateTime.Offset.Equals(TimeZoneInfo.Local.GetUtcOffset(dateTime.DateTime)))
                return DateTime.SpecifyKind(dateTime.DateTime, DateTimeKind.Local);
            else
                return dateTime.DateTime;
        }
    }
}
