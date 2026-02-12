using System;
namespace Demo_PIG_Tool.Utils
{ 
    public static class HealthText
    {


        //For phase 1, early demo purposes, clears the CLI screen for better readability
        public static void ClearScreen()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Title = "Health Tracking Tool";
            Console.ResetColor();

        }

        public static void Greetings()
        {
            ClearScreen();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine
            (   "\n\n"+
                "\t\t\t+------------------------------------------------------------------------------------+\n" +
                "\t\t\t|                                HEALTH TRACKING                                     |\n" +
                "\t\t\t|                                                                                    |\n" +
                "\t\t\t|   This tool allows logging of daily calories consumed and daily weight tracking.   |\n" +
                "\t\t\t|   Utilize the menu to navigate through the tool.                                   |\n" +
                "\t\t\t+------------------------------------------------------------------------------------+\n\n"
            );
            Console.ResetColor();
        }

        public static void ShowMenu()
        {
            //symbols from: https://stackoverflow.com/questions/71912239/c-sharp-console-is-outputting-box-characters-as


            string menuPrompt = 
            @"
              ╔════════════════════ MENU ════════════════════╗
              ║                                              ║
              ║   [1] View Health Logs                       ║
              ║   [2] Input Today's Health Data              ║ 
              ║   [3] Modify Existing Logs                   ║
              ║   [4] Return to Health Tool Menu             ║
              ║   [5] Return to Tool Directory               ║
              ║                                              ║
              ╠══════════════════════════════════════════════╣ 
              ║  Type  your choice and press ENTER           ║
              ╚══════════════════════════════════════════════╝
              ";

            string[] lines = menuPrompt.Split('\n'); // Split the menu prompt into individual lines, 
                                                     // to calculate its dimensions for positioning
            int menuHeight = lines.Length;
            int menuWidth  = lines[0].Length;

            int screenWidth  = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;

            int rightPadding  = 8; //modify these numbers to adjust menu position
            int bottomPadding = 5;

            int startX = Math.Max(0, screenWidth  - menuWidth  - rightPadding); // Uses "SetCursorPosition" to align the menu in a specific area 
            int startY = Math.Max(0, screenHeight - menuHeight - bottomPadding);

            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(startX, startY + i);
                Console.Write(lines[i]);
            }
            
        }

        public static void FeatureComingSoon()
        {
            ClearScreen();
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine   //from: https://patorjk.com/software/taag/

            (@"


                ███████████                     █████                                       █████████                            ███                          █████████                                
               ░░███░░░░░░█                    ░░███                                       ███░░░░░███                          ░░░                          ███░░░░░███                               
                ░███   █ ░   ██████   ██████   ███████   █████ ████ ████████   ██████     ███     ░░░   ██████  █████████████   ████  ████████    ███████   ░███    ░░░   ██████   ██████  ████████    
                ░███████    ███░░███ ░░░░░███ ░░░███░   ░░███ ░███ ░░███░░███ ███░░███   ░███          ███░░███░░███░░███░░███ ░░███ ░░███░░███  ███░░███   ░░█████████  ███░░███ ███░░███░░███░░███   
                ░███░░░█   ░███████   ███████   ░███     ░███ ░███  ░███ ░░░ ░███████    ░███         ░███ ░███ ░███ ░███ ░███  ░███  ░███ ░███ ░███ ░███    ░░░░░░░░███░███ ░███░███ ░███ ░███ ░███   
                ░███  ░    ░███░░░   ███░░███   ░███ ███ ░███ ░███  ░███     ░███░░░     ░░███     ███░███ ░███ ░███ ░███ ░███  ░███  ░███ ░███ ░███ ░███    ███    ░███░███ ░███░███ ░███ ░███ ░███   
                █████      ░░██████ ░░████████  ░░█████  ░░████████ █████    ░░██████     ░░█████████ ░░██████  █████░███ █████ █████ ████ █████░░███████   ░░█████████ ░░██████ ░░██████  ████ █████  
               ░░░░░        ░░░░░░   ░░░░░░░░    ░░░░░    ░░░░░░░░ ░░░░░      ░░░░░░       ░░░░░░░░░   ░░░░░░  ░░░░░ ░░░ ░░░░░ ░░░░░ ░░░░ ░░░░░  ░░░░░███    ░░░░░░░░░   ░░░░░░   ░░░░░░  ░░░░ ░░░░░  
                                                                                                                                                 ███ ░███                                              
                                                                                                                                                ░░██████                                               
                                                                                                                                                ░░░░░░                                                 
            ");

            Console.ResetColor();
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
            Greetings();
        } 

        public static void LogHealthData(int part)
        {
            if (part == 1){          
                Console.WriteLine("~~~ Log Today's Health Data ~~~\n");
                Console.WriteLine("\tToday's Date: " + UtilsDate.GetDate());
            }
            else if (part == 2)
                Console.Write("\tWeight (lbs): ");
            else if (part == 3)
                Console.Write("\tCalories Consumed: ");
            else if(part == 4)
                Console.WriteLine("\nData logged successfully!: ");
        }
                

    } 
}
