namespace CloudConsult.UI.Blazor.Common
{
    public static class DateTimeExtensions
    {
		public static DateTime RoundUpToNearest30(this DateTime datetime)
		{
			double atMinuteInBlock = datetime.TimeOfDay.TotalMinutes % 30;
			double minutesToAdd = 30 - atMinuteInBlock;
			return datetime.AddMinutes(minutesToAdd);
		}

		public static DateTime RoundUpToNearest(this DateTime datetime, int roundToMinutes)
		{
			double minutes = datetime.TimeOfDay.TotalMinutes % roundToMinutes;
			double minutesToAdd = roundToMinutes - minutes;
			return datetime.AddMinutes(minutesToAdd);
		}

		public static DateTime RoundDownToNearest30(this DateTime datetime)
		{
			double minutes = datetime.TimeOfDay.TotalMinutes % 30;
			return datetime.AddMinutes(-minutes);
		}
	}
}
