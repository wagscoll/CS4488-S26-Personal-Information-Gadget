using System;
using System.IO;
using Health = Demo_PIG_Tool.HealthTool.HealthTool;
using Budget = Demo_PIG_Tool.BudgetTool.BudgetDemo;
using Project = Demo_PIG_Tool.ProjectTool.ProjectTool;
using Demo_PIG_Tool.BudgetTool;


namespace Demo_PIG_Tool.Manager
{


    public static class SubToolManager
    {
        public static void Run()
        {
            while (true)
            {

                Greetings();
                ShowMenu();

                Console.Write("\nChoice: ");
                var input = Console.ReadLine();

                if (!int.TryParse(input, out int navigationChoice))
                {

                    Console.WriteLine("Please enter a number (1-5). Press ENTER to try again.");
                    Console.ReadLine();
                    continue;
                }

                switch (navigationChoice)
                {

                    case 1:
                        Health.Run();
                        break;
                    case 2:
                        Budget.Run();
                        break;
                    case 3:
                        Project.Run();
                        break;
                    case 4:
                        // View All Logs functionality added here
                        PrintAllLogs();
                        break;
                    case 5:
                        Console.WriteLine("Exiting program. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please select 1-4. Press ENTER to try again.");
                        Console.ReadLine();
                        break;
                }

                Console.WriteLine("\nPress ENTER to return to the main menu...");
                Console.ReadLine();
            }
        }

        public static void Greetings()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.Title = "Demo PIG Tool";
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;

            String welcomeMessage =
            @"

                                ╔══════════════════════════ WELCOME TO THE PIG TOOL DEMO ══════════════════════════╗
                                ║                                                                                  ║
                                ║       This tool is a demonstration showcasing several built-in features.         ║
                                ║       Navigate through the menu to explore the various functionalities.          ║
                                ║                                                                                  ║
                                ╚══════════════════════════════════════════════════════════════════════════════════╝

            ";
            Console.WriteLine(welcomeMessage);
            Console.ResetColor();
        }


        public static void ShowMenu()
        {
            //symbols from: https://stackoverflow.com/questions/71912239/c-sharp-console-is-outputting-box-characters-as

            string menuPrompt =
            @"
              ╔════════════════════ MENU ════════════════════╗
              ║                                              ║
              ║   [1] Health Tracking - Weight and Calories  ║
              ║   [2] Budget Tracking - Expenses and Income  ║ 
              ║   [3] Project & Task Tool                    ║
              ║   [4] View All Logs                          ║              
              ║   [5] Exit Program                           ║
              ║                                              ║
              ╠══════════════════════════════════════════════╣ 
              ║  Type  your choice and press ENTER           ║
              ╚══════════════════════════════════════════════╝
              ";

            string[] lines = menuPrompt.Split('\n'); // Split the menu prompt into individual lines, 
                                                     // to calculate its dimensions for positioning
            int menuHeight = lines.Length;
            int menuWidth = lines[0].Length;

            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;

            int rightPadding = 8; //modify these numbers to adjust menu position
            int bottomPadding = 5;

            int startX = Math.Max(0, screenWidth - menuWidth - rightPadding); // Uses "SetCursorPosition" to align the menu in a specific area 
            int startY = Math.Max(0, screenHeight - menuHeight - bottomPadding);

            for (int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(startX, startY + i);
                Console.Write(lines[i]);
            }
        }

        private static void PrintAllLogs()
        {
            Console.Clear();
            Console.WriteLine("--- All Logs ---\n");

            string basePath = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "logs"));

            string healthPath = Path.Combine(basePath, "healthlogs.txt");
            string projectPath = Path.Combine(basePath, "projectsAndTasksLogs.txt");
            // To add budget logs in the future when implemented
            //string budgetPath = Path.Combine(basePath, "budgetlogs.txt"); 

            Console.WriteLine("--- Health Logs ---");
            PrintFileContents(healthPath);

            Console.WriteLine("\n--- Projects and Tasks ---");
            PrintFileContents(projectPath);

            // Uncomment the following lines when budget logs are implemented
            /*
            Console.WriteLine("\n--- Budget Logs ---");
            PrintFileContents(budgetPath);
            */
        }

        private static void PrintFileContents(string path)
        {
            if (File.Exists(path))
            {
                Console.WriteLine(File.ReadAllText(path));
            }
            else
            {
                Console.WriteLine("(Log file not found)");
            }
        }





    }
}
