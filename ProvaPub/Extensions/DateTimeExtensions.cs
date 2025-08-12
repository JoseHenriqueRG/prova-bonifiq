using System.Runtime.InteropServices;

namespace ProvaPub.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly TimeZoneInfo BrazilTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "E. South America Standard Time"
                    : "America/Sao_Paulo");

        public static DateTime ToBrazilTime(this DateTime utcDate)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDate, BrazilTimeZone);
        }
    }
}
