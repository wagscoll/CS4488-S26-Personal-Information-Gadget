using System;
using System.Globalization;

namespace Demo_PIG_Tool.Utils
{
    public static class InputValidation
    {
        public static bool IsValidDate(string input)
        {
            return DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        public static bool IsValidFloat(float value, float min = 0, float max = 100000)
        {
            return !float.IsNaN(value) && value >= min && value <= max;
        }
    }
}
