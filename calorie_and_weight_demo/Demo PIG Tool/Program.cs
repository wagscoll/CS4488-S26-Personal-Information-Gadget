
// Calorie Tracker and Weight Tracker Demo
global using HealthLog = (int date, float weight, float calories);
using System;
using System.IO;
using System.Collections.Generic;
using Demo_PIG_Tool.Utils;




List<UtilsProject> projects = new();
List<UtilsTask> tasks = new();

/*  1/28/26 - Collin

        Work Flow:
        1. greetings()
            2. terminalMenu()
                3. Options:
                    A. displayHealthLogs()
                        i. getHealthLogs() --> reads from healthlogs.txt and populates healthLogs list
                    B. inputNewHealthData()
                        i. collect data via:
                            a. logDate()
                            b. logWeight()
                            c. logCalories()
                        ii. submitHealthData() --> appends new data to healthlogs.txt

*/

//TODO:
//  create 'log' objects that hold date, weight, calories data 





/*--------------------------------------Text Prompts and Menu Traversal--------------------------------------*/

void terminalMenu()
{   
    UtilsText.ShowMenu();

    int choice1;
    choice1 = int.Parse(Console.ReadLine());

    switch (choice1)
    {
        case 1:
            UtilsText.ClearScreen();
            displayProjectsAndTasks();
            break;
        case 2:
            UtilsText.ClearScreen();
            createATask();
            break;
        case 3:
            UtilsText.ClearScreen();
            createAProject();
            terminalMenu();
            break;
        case 4:
            UtilsText.ClearScreen();
            UtilsText.Greetings();
            terminalMenu();
            break;
        default:
            UtilsText.ClearScreen();
            UtilsText.Greetings();
            terminalMenu();
            break;
    }
}

void displayProjectsAndTasks()
{
    UtilsText.ClearScreen();
    Console.WriteLine("\t\t --- Projects and Tasks ---\n");

    string projectsAndTasksLogs = getProjectsAndTasksLogs();    // populate healthLogs list
    Console.WriteLine(projectsAndTasksLogs);          // display healthLogs data

    terminalMenu();
}
/*-----------------------------------------------------------------------------------------------------------*/



string GetProjectsAndTasksLogPath()
{
    return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "projectsAndTasksLogs.txt"));
}

string getProjectsAndTasksLogs()
{
    string path = GetProjectsAndTasksLogPath();
    if (File.Exists(path))
    {
        string readText = File.ReadAllText(path);
        return readText;
    }

    Console.WriteLine("Log file not found.");
    return "";
}

void loadData()
{
    string path = GetProjectsAndTasksLogPath();
    if (File.Exists(path))
    {
        using(StreamReader sr = File.OpenText(path))
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
                        float.Parse(entries[6])
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
                        int.Parse(entries[7])
                    ));
                }
            }
        }
    }
}



//---------------------------------------------Path & .txt Functions---------------------------------------------
void createATask()
{
    UtilsText.CreateATaskOrProject("tname");
    string taskName = Console.ReadLine();
    UtilsText.CreateATaskOrProject("timportant");
    bool isImportant = Console.ReadLine().ToLower() == "y";
    UtilsText.CreateATaskOrProject("turgent");
    bool isUrgent = Console.ReadLine().ToLower() == "y";
    UtilsText.CreateATaskOrProject("tdue");
    DateTime dueDate = DateTime.Parse(Console.ReadLine());
    UtilsText.CreateATaskOrProject("thours");
    float estimatedHours = float.Parse(Console.ReadLine());
    UtilsText.CreateATaskOrProject("tpartproject");
    bool partOfProject = Console.ReadLine().ToLower() == "y";
    int projectId = -1;
    if (partOfProject)
    {
        UtilsText.CreateATaskOrProject("tprojectname");
        string projectname = Console.ReadLine();
        foreach (UtilsProject project in projects)
        {
            if (projectname == project.GetProjectName())
            {
                projectId = project.GetProjectId();
            }
        }
    }

    tasks.Add(new UtilsTask(tasks.Count + 1, taskName, isImportant, isUrgent, dueDate, estimatedHours, projectId));
    saveChanges();
    terminalMenu();
}

void createAProject()
{
    UtilsText.CreateATaskOrProject("pname");
    string name = Console.ReadLine();
    UtilsText.CreateATaskOrProject("pimportant");
    bool isImportant = Console.ReadLine().ToLower() == "y";
    UtilsText.CreateATaskOrProject("purgent");
    bool isUrgent = Console.ReadLine().ToLower() == "y";
    UtilsText.CreateATaskOrProject("pdue");
    DateTime dueDate = DateTime.Parse(Console.ReadLine());
    UtilsText.CreateATaskOrProject("phours");
    float estimatedHours = float.Parse(Console.ReadLine());
    

    projects.Add(new UtilsProject(projects.Count + 1, name, isImportant, isUrgent, dueDate, estimatedHours));
    saveChanges();
    terminalMenu();
}

void saveChanges()
{
    string path = GetProjectsAndTasksLogPath();
    using (StreamWriter sw = File.CreateText(path))
    {
        foreach (UtilsProject project in projects)
        {
            sw.WriteLine($"PROJECT|{project.GetProjectId()}|{project.GetProjectName()}|{project.getisImportant()}|{project.getisUrgent()}|{project.getDueDate()}|{project.getEstimatedHours()}");
        }
        foreach (UtilsTask task in tasks)
        {
            sw.WriteLine($"TASK|{task.GetTaskId()}|{task.GetTaskName()}|{task.getisImportant()}|{task.getisUrgent()}|{task.getDueDate()}|{task.getEstimatedHours()}|{task.getProjectId()}");
        }
    }
}
/*---------------------------------------------------------------------------------------------------------*/



loadData();
UtilsText.Greetings();
terminalMenu();


    