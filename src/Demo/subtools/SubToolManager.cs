using System;
using System.IO;
using Health = Demo_PIG_Tool.HealthTool.HealthTool;
using Budget = Demo_PIG_Tool.BudgetTool.BudgetDemo;
using Demo_PIG_Tool.BudgetTool;
using Demo_PIG_Tool.Utils;
using System.Net;


namespace Demo_PIG_Tool.Manager
{
    public static class SubToolManager
    {

        private static List<UtilsProject> projects = new();
        private static List<UtilsTask> tasks = new();
        public static void Run()
        {
            // DEPRECIATED - CLI Prototype
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
                    // Runs the health tracking tool
                    case 1:
                        Health.Run();
                        break;

                    // Runs the budget tracking tool
                    case 2:
                        Budget.Run();
                        break;

                    // Runs the project and task management tool
                    case 3:
                        //Project.Run();
                        break;

                    //Print all logs to console
                    case 4:
                        PrintAllLogs();
                        break;

                    // Exiting the program
                    case 5:
                        Console.WriteLine("Exiting program. Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please select 1-5. Press ENTER to try again.");
                        Console.ReadLine();
                        break;
                }

                Console.WriteLine("\nPress ENTER to return to the main menu...");
                Console.ReadLine();
            }
        }

        // DEPRECIATED - CLI Prototype
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

        // DEPRECIATED - CLI Prototype
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
            string budgetPath = Path.Combine(basePath, "budgetlogs.txt");
            string shoppingPath = Path.Combine(basePath, "shoppingLogs.txt");

            Console.WriteLine("--- Health Logs ---");
            PrintFileContents(healthPath);

            Console.WriteLine("\n--- Projects and Tasks ---");
            PrintFileContents(projectPath);

            Console.WriteLine("\n--- Budget Logs ---");
            PrintFileContents(budgetPath);

            Console.WriteLine("\n--- Shopping List Logs ---");
            PrintFileContents(shoppingPath);
        }

        private static string GetHealthLogs()
        {
            string basePath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "logs"));

            string healthPath = Path.Combine(basePath, "healthlogs.txt");

            const string header = "--- Health Logs ---\nDate       | Weight  | Calories\n----------------------------------\n";

            if (!File.Exists(healthPath))
                return header + "(Health log file not found)";
            else return header + File.ReadAllText(healthPath); 
        }
        //anh 4/30/26 - adds tags to projects and tasks output
        private static string Tag(bool important, bool urgent)
        {
            string tag = "";
            if (important) tag += " [I]";
            if (urgent) tag += " [U]";
            return tag;
        }
        //anh 4/30/26 - loads in the data from the logs file
        private static void loadData(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] entries = s.Split('|');
                        if (entries[0] == "PROJECT")
                        {
                            projects.Add(new UtilsProject(
                                int.Parse(entries[1]),
                                entries[2],
                                bool.Parse(entries[3]),
                                bool.Parse(entries[4]),
                                DateTime.Parse(entries[5]),
                                float.Parse(entries[6]),
                                entries[7]
                            ));
                        }
                        else if (entries[0] == "TASK")
                        {
                            tasks.Add(new UtilsTask(
                                int.Parse(entries[1]),
                                entries[2],
                                bool.Parse(entries[3]),
                                bool.Parse(entries[4]),
                                DateTime.Parse(entries[5]),
                                float.Parse(entries[6]),
                                int.Parse(entries[7]),
                                entries[8]
                            ));
                        }
                    }
                }
            }
        }

        //anh 4/30/26 - gets the data from the log file and then formats it in a user friendly way
        private static string GetProjectLogs()
        {
            string basePath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "logs"));

            string projectPath = Path.Combine(basePath, "projectsAndTasksLogs.txt");

            if (!File.Exists(projectPath)) { return "--- Projects and Tasks Logs ---\n(Projects and Tasks log file not found)"; }

            loadData(projectPath);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("--- Projects and Tasks Logs ---");

            var allDates = projects.Select(p => p.getDueDate().Date)
                .Union(tasks.Select(t => t.getDueDate().Date))
                .OrderBy(d => d);

            foreach (var date in allDates)
            {
                sb.AppendLine($"{date:M/d/yyyy}");

                var times = projects.Select(p => p.getDueDate())
                    .Concat(tasks.Select(t => t.getDueDate()))
                    .Where(d => d.Date == date)
                    .Select(d => d.TimeOfDay)
                    .Distinct()
                    .OrderBy(t => t);

                foreach (var time in times)
                {
                    sb.AppendLine($"\t{DateTime.Today.Add(time):hh:mm:ss tt}");

                    // TASKS FIRST
                    var tasksAtTime = tasks
                        .Where(t => t.getDueDate().Date == date &&
                                    t.getDueDate().TimeOfDay == time)
                        .OrderByDescending(t => t.getisUrgent())
                        .ThenByDescending(t => t.getisImportant())
                        .ThenBy(t => t.GetTaskName());

                    foreach (var task in tasksAtTime)
                    {
                        string projLabel = "";
                        if (task.getProjectId() != -1)
                        {
                            var proj = projects.FirstOrDefault(p => p.GetProjectId() == task.getProjectId());
                            if (proj != null)
                                projLabel = $" (Task of {proj.GetProjectName()})";
                        }

                        sb.AppendLine($"\t\t{task.GetTaskName()}{projLabel}{Tag(task.getisImportant(), task.getisUrgent())}");
                        sb.AppendLine($"\t\t\tEst. Hours: {task.getEstimatedHours()}");

                        if (!string.IsNullOrWhiteSpace(task.getNotes()))
                            sb.AppendLine($"\t\t\t{task.getNotes()}");
                    }

                    // PROJECTS
                    var projectsAtTime = projects
                        .Where(p => p.getDueDate().Date == date &&
                                    p.getDueDate().TimeOfDay == time)
                        .OrderByDescending(p => p.getisUrgent())
                        .ThenByDescending(p => p.getisImportant())
                        .ThenBy(p => p.GetProjectName());

                    foreach (var proj in projectsAtTime)
                    {
                        sb.AppendLine($"\t\t{proj.GetProjectName()} [P]{Tag(proj.getisImportant(), proj.getisUrgent())}");
                        sb.AppendLine($"\t\t\tEst. Hours: {proj.getEstimatedHours()}");

                        if (!string.IsNullOrWhiteSpace(proj.getNotes()))
                            sb.AppendLine($"\t\t\t{proj.getNotes()}");

                        // SUB-TASKS
                        var subTasks = tasks
                            .Where(t => t.getProjectId() == proj.GetProjectId())
                            .ToList();

                        if (subTasks.Any())
                        {
                            sb.AppendLine("\t\t\tSub-Tasks:");
                            foreach (var t in subTasks)
                            {
                                sb.AppendLine($"\t\t\t- {t.GetTaskName()}");
                            }
                        }
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string GetBudgetLogs()
        {
            string basePath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "logs"));

            string budgetPath = Path.Combine(basePath, "budgetLogs.txt");

            if(!File.Exists(budgetPath))
                return "--- Budget Logs ---\n(Budget log file not found)";
             else
                return "--- Budget Logs ---\n" + File.ReadAllText(budgetPath);
        }

        private static string GetShoppingLogs()
        {
            string basePath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "logs"));

            string shoppingPath = Path.Combine(basePath, "shoppingLogs.txt");

            if (!File.Exists(shoppingPath))
                return "--- Shopping Logs ---\n(Shopping log file not found)";
            else
                return "--- Shopping Logs ---\n" + File.ReadAllText(shoppingPath);
        }


        // ~ Collin Wagstaff
        // UpdateDocx acts as an auto-save - it is called at the beginning and end of the program to ensure 
        // that the Output.docx file is always up to date with the latest logs
        public static void UpdateDocx()
        {
            string s1 = GetHealthLogs();
            string s2 = GetProjectLogs();
            string s3 = GetBudgetLogs();
            string s4 = GetShoppingLogs();
            string combined = s1 + "\n\n" + s2 + "\n\n" + s3 + "\n\n" + s4;

            try
            {
                Docx.WriteToDocx("Output.docx", combined); // creates/overwrites
                Console.WriteLine("Wrote Output.docx");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to docx: {ex.Message}");
            }
        }

        private static void PrintFileContents(string path)
        {
            if (File.Exists(path))
                Console.WriteLine(File.ReadAllText(path));
            else
                Console.WriteLine("(Log file not found)");
        }
    }
}
