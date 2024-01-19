using System.Globalization;

namespace SimbirSoft.Services.Implementations
{
    public static class IsDateTime
    {
        public static bool AreDatesISO0861(DateTime startDateTime, DateTime endDateTime)
        {
            string format = "o"; // Round-trip ("O", "o") format specifier
            CultureInfo culture = CultureInfo.InvariantCulture; // Use the invariant culture

            string dateString1 = startDateTime.ToString(format, culture);
            string dateString2 = endDateTime.ToString(format, culture);

            DateTime parsedDateTime1;
            DateTime parsedDateTime2;

            bool isValidParam1 = DateTime.TryParseExact(dateString1, format, culture, DateTimeStyles.RoundtripKind, out parsedDateTime1);
            bool isValidParam2 = DateTime.TryParseExact(dateString2, format, culture, DateTimeStyles.RoundtripKind, out parsedDateTime2);

            return isValidParam1 && isValidParam2;
        }
    }
}
