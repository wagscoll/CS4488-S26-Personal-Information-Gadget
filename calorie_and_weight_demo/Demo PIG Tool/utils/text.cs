

using System;
using Demo_PIG_Tool.Utils;
namespace Demo_PIG_Tool.Utils
{ 
    public static class UtilsText
    {



        //For phase 1, early demo purposes, clears the CLI screen for better readability
        public static void ClearScreen()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Title = "Project and Task Management Tool";
            Console.ResetColor();

        }

        public static void Greetings()
        {
            ClearScreen();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine
            (   "\n\n"+
                "\t\t\t+------------------------------------------------------------------------------------+\n" +
                "\t\t\t|                                Project and Task Management Tool                    |\n" +
                "\t\t\t|                                                                                    |\n" +
                "\t\t\t|   This tool allows the creation of projects and tasks.                             |\n" +
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
              ║   [1] View and Edit Tasks and Projects       ║
              ║   [2] Create a Task                          ║ 
              ║   [3] Create a Project                       ║
              ║   [4] Return to Main Menu                    ║
              ║                                              ║
              ╠══════════════════════════════════════════════╣ 
              ║  Type  your choice and press ENTER           ║
              ╚══════════════════════════════════════════════╝
              ";

            string[] lines = menuPrompt.Split('\n');
            int menuHeight = lines.Length;
            int menuWidth  = lines[0].Length;

            int screenWidth  = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;

            int rightPadding  = 8;
            int bottomPadding = 5;

            int startX = Math.Max(0, screenWidth  - menuWidth  - rightPadding);
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

        public static void CreateATaskOrProject(string part)
        {
            if (part == "tname")
            {
                Console.WriteLine("~~~ Create A Task ~~~\n");
                Console.Write("\tTask's Name: ");
            }
            else if (part == "pname")
            {
                Console.WriteLine("~~~ Create A Project ~~~\n");
                Console.Write("\tProject's Name: ");
            }
            else if (part == "timportant")
                Console.Write("\tIs this task important (y/n): ");
            else if (part == "turgent")
                Console.Write("\tIs this task urgent (y/n): ");
            else if (part == "tdue")
                Console.Write("\tWhen is this task due (yyyy-mm-dd HH:MM AM or PM): ");
            else if (part == "thours")
                Console.Write("\tHow many hours will this task take: ");
            else if (part == "tpartproject")
                Console.Write("\tIs this task part of an Ongoing Project (y/n): ");
            else if (part == "tprojectname")
                Console.Write("\tWhat is the Project's Name: ");
            else if (part == "tcreated")
                Console.WriteLine("\nTask created successfully! ");
            else if (part == "pcreated")
                Console.WriteLine("\nProject created successfully! ");
            else if (part == "pimportant")
                Console.Write("\tIs this project important (y/n): ");
            else if (part == "purgent")
                Console.Write("\tIs this project urgent (y/n): ");
            else if (part == "pdue")
                Console.Write("\tWhen is this project due (yyyy-mm-dd HH:MM AM or PM): ");
            else if (part == "phours")
                Console.Write("\tHow many hours will this project take: ");
        }
                

    } 
}
