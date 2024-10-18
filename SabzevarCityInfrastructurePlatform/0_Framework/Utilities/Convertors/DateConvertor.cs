using System.Globalization;

namespace _0_Framework.Utilities.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00");
        }

        public static string ToShamsiWithTime(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00") + " - " +
                   pc.GetMinute(value).ToString("00") + " : " + pc.GetHour(value).ToString("00");
        }
        public static int GetMonthOfDate(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetMonth(value);
        }

        public static int GetYearOfDate(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value);
        }


        public static string ToShamsiWithTime(this DateTime? date)
        {
            PersianCalendar pc = new PersianCalendar();
            DateTime value = date.GetValueOrDefault();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00") + " - " +
                   pc.GetMinute(value).ToString("00") + " : " + pc.GetHour(value).ToString("00");
        }

    }
}
