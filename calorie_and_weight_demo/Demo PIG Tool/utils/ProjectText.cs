

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

        public static void ShowMenu(string type)
        {
            if (type == "main")
            {
                //symbols from: https://stackoverflow.com/questions/71912239/c-sharp-console-is-outputting-box-characters-as

                // ANH - Changed menu options to match the current functionality of the tool, and added a new option to view and edit existing tasks and projects.
                string menuPrompt =
                @"
              ╔════════════════════ MENU ════════════════════╗
              ║                                              ║
              ║   [1] View and Edit Tasks and Projects       ║
              ║   [2] Create a Task                          ║ 
              ║   [3] Create a Project                       ║
              ║   [4] View Next Two Weeks                    ║
              ║   [5] Return to Tool Directory               ║
              ║                                              ║
              ╠══════════════════════════════════════════════╣ 
              ║  Type  your choice and press ENTER           ║
              ╚══════════════════════════════════════════════╝
              ";

                string[] lines = menuPrompt.Split('\n');
                int menuHeight = lines.Length;
                int menuWidth = lines[0].Length;

                int screenWidth = Console.WindowWidth;
                int screenHeight = Console.WindowHeight;

                int rightPadding = 8;
                int bottomPadding = 5;

                int startX = Math.Max(0, screenWidth - menuWidth - rightPadding);
                int startY = Math.Max(0, screenHeight - menuHeight - bottomPadding);

                for (int i = 0; i < lines.Length; i++)
                {
                    Console.SetCursorPosition(startX, startY + i);
                    Console.Write(lines[i]);
                }
                //Added the view and edit option to the main menu, but it will not be visible until the user selects it from the main menu. This is because the view and edit menu is only shown when the user selects that option from the main menu, and it is not shown by default when the user first opens the tool.
            }
            else if (type == "viewandedit")
            {
                //symbols from: https://stackoverflow.com/questions/71912239/c-sharp-console-is-outputting-box-characters-as


                string menuPrompt =
                @"
              ╔════════════════════ MENU ════════════════════╗
              ║                                              ║
              ║   [-1] Return to Main Menu                   ║
              ║   [-2] Create a Task                         ║ 
              ║   [-3] Create a Project                      ║
              ║   [ID Number] Edit that Task or Project      ║
              ║                                              ║
              ╠══════════════════════════════════════════════╣ 
              ║  Type  your choice and press ENTER           ║
              ╚══════════════════════════════════════════════╝
              ";

                string[] lines = menuPrompt.Split('\n');
                int menuHeight = lines.Length;
                int menuWidth = lines[0].Length;

                int screenWidth = Console.WindowWidth;
                int screenHeight = Console.WindowHeight;

                int rightPadding = 8;
                int bottomPadding = 5;

                int startX = Math.Max(0, screenWidth - menuWidth - rightPadding);
                int startY = Math.Max(0, screenHeight - menuHeight - bottomPadding);

                for (int i = 0; i < lines.Length; i++)
                {
                    Console.SetCursorPosition(startX, startY + i);
                    Console.Write(lines[i]);
                }
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
        public static void EditATaskOrProject(string type, UtilsProject project, UtilsTask task, string projectName)
        {
            if(type == "both")
            {
                ClearScreen();
                Console.WriteLine("~~~ Edit Task or Project ~~~\n");
                Console.WriteLine("Which are you trying to edit?");
                Console.WriteLine("\t[1] Edit Project: " + project.GetProjectName());
                Console.WriteLine("\t[2] Edit Task: " + task.GetTaskName());
                Console.WriteLine("\t[3] Neither");
            }
            else if(type == "project")
            {                 
                ClearScreen();
                Console.WriteLine("~~~ Edit Project ~~~\n");
                Console.WriteLine("Editing Project: " + project.GetProjectName() + "\n");
                Console.WriteLine("\t[1] Edit Project Name?");
                Console.WriteLine("\t[2] Edit Is Important? Currently " + project.getisImportant());
                Console.WriteLine("\t[3] Edit Is Urgent? Currently " + project.getisUrgent());
                Console.WriteLine("\t[4] Edit Due Date? Currently " + project.getDueDate());
                Console.WriteLine("\t[5] Edit Estimated Hours? Currently " + project.getEstimatedHours());
                Console.WriteLine("\t[6] Delete Project?");
                Console.WriteLine("\t[7] Return to previous menu?");
            }
            else if(type == "task")
            {                 
                ClearScreen();
                Console.WriteLine("~~~ Edit Task ~~~\n");
                Console.WriteLine("Editing Task: " + task.GetTaskName() + "\n");
                Console.WriteLine("\t[1] Edit Task Name?");
                Console.WriteLine("\t[2] Edit Is Important? Currently " + task.getisImportant());
                Console.WriteLine("\t[3] Edit Is Urgent? Currently " + task.getisUrgent());
                Console.WriteLine("\t[4] Edit Due Date? Currently " + task.getDueDate());
                Console.WriteLine("\t[5] Edit Estimated Hours? Currently " + task.getEstimatedHours());
                Console.WriteLine("\t[6] Edit Project this task is apart of? Currently " + projectName);
                Console.WriteLine("\t[7] Delete Task?");
                Console.WriteLine("\t[8] Return to previous menu?");
            }
            else if(type == "deleteProject")
            {
                ClearScreen();
                Console.WriteLine("~~~ Delete Project ~~~\n");
                Console.WriteLine("Do you want to delete all of " + project.GetProjectName() + "'s sub tasks as well? (y/n)");
            }
        }

    } 
}