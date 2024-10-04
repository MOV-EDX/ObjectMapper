namespace ObjectMapper.Extensions
{
    internal static class DateTimeExtensions
    {
        public static object ToDateTimeOffset(this DateTime dateTime)
        {
            if (dateTime.Kind is DateTimeKind.Unspecified)
            {
                return new DateTimeOffset(dateTime, new TimeSpan(0, 0, 0));
            }

            return new DateTimeOffset(dateTime);
        }
    }
}
