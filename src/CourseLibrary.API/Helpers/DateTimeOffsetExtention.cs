using System;

namespace CourseLibrary.API.Helpers
{
    public static class DateTimeOffsetExtention
    {
        public static int GetCurrentAge(this DateTimeOffset dateTimeOffset)
        {
            var currentData = DateTime.UtcNow;
            int age = currentData.Year - dateTimeOffset.Year;

            if (currentData < dateTimeOffset.AddYears(age))
            {
                age--;
            }
            return age;
        }
    }
}
