namespace Common.Helpers
{
    public static class DateTimeHelper
    {
        // Đặt múi giờ cho Việt Nam (+7)
        private static readonly TimeZoneInfo VietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        // Phương thức để lấy thời gian hiện tại ở Việt Nam
        public static DateTime Now()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, VietnamTimeZone);
        }
    }
}
