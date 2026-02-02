using System;




namespace Demo_PIG_Tool.Utils
{       
    public static class UtilsDate
    {
        public static string GetDate()
        {
            DateTime now = DateTime.Now;
            string currentDate = now.ToString("yyyy-MM-dd");
            return currentDate;
        }





    }
}
