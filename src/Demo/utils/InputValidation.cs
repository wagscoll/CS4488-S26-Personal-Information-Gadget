using System;

namespace Demo_PIG_Tool.Utils
{

    /*
    public static class InputValidation
    {
        // TryParseExact - found at: https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tryparseexact?view=net-10.0
        public static string ValidateDateInput(string input)
        {
            // Loops until the user enters a valid date in the format "yyyy-MM-dd". 
            while (!DateTime.TryParseExact( input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                string? line = Console.ReadLine();
                if (line == null)
                    input = "";
                else
                    input = line;
            }

            return input;
        }
    }
    */
}