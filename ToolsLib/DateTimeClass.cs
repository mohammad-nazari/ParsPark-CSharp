using System;
using System.Globalization;

namespace ToolsLib
{
	public class DateTimeClass
	{

		public static string GetDuration(DateTime StartTime, DateTime EndTime)
		{
			TimeSpan span = (EndTime - StartTime);

			return $"{span.Days} days, {span.Hours} hours, {span.Minutes} minutes, {span.Seconds} seconds";
		}

		public static string ToPersianFormat(DateTime GeoDateTime, string FormatString = "yyyy/MM/dd HH:mm")
		{
			PersianCalendar farsiDateTime = new PersianCalendar();
			FormatString = FormatString.Replace("yyyy", farsiDateTime.ToFourDigitYear(farsiDateTime.GetYear(GeoDateTime)).ToString("D4"));
			FormatString = FormatString.Replace("dd", farsiDateTime.GetDayOfMonth(GeoDateTime).ToString("D2"));
			FormatString = FormatString.Replace("MM", farsiDateTime.GetMonth(GeoDateTime).ToString("D2"));
			FormatString = FormatString.Replace("HH", farsiDateTime.GetHour(GeoDateTime).ToString("D2"));
			FormatString = FormatString.Replace("mm", farsiDateTime.GetMinute(GeoDateTime).ToString("D2"));
			FormatString = FormatString.Replace("ss", farsiDateTime.GetSecond(GeoDateTime).ToString("D2"));

			return FormatString;
		}

		public static DateTime SetTime(DateTime CurrentDateTime, int Nour, int Minute, int Second)
		{
			return CurrentDateTime.Date + new TimeSpan(Nour, Minute, Second);
		}
	}
}
