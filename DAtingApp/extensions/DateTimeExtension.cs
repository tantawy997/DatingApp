namespace DAtingApp.extensions
{
	public static class DateTimeExtension
	{
		public static int CalculateAge(this DateTime DOB)
		{
			var today = DateTime.UtcNow;

			int age = today.Year - DOB.Year;

			if (DOB > today.AddYears(-age)) age--;

			return age;

		}
	}
}
