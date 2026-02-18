using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Demo_PIG_Tool.Utils;
using Demo_PIG_Tool.Manager;

namespace Demo_PIG_Tool.ProjectTool;


public static class ProjectTool
{
    public static void Run()
    {
        new Program().Run();

    }

    class Program
    {


    List<UtilsProject> projects = new();
    List<UtilsTask> tasks = new();

    /*  2/2/26 - Alex Henderson

            Work Flow:
            1. greetings()
                2a. loadData() --> loads data from .txt file into projects and tasks lists
                2b. terminalMenu()
                    3. Options:
                        A. "main"
                            i. displayProjectsAndTasks() --> Lets the user view all tasks and projects
                                a. getProjectsAndTasksLogs() --> reads from .txt file and returns string to display
                                    1. GetProjectsAndTasksLogPath() --> gets path to .txt file
                                b. termainalMenu("viewandedit") --> lets user edit or return to main menu
                            ii. createATask() --> Asks for relavent info and creates a task and stores in tasks list
                            iii. createAProject() --> Asks for relavent info and creates a project and stores in projects list
                        B. "viewandedit"
                            i. displayProjectsAndTasks()
                            ii. createATask()
                            iii. createAProject()
                            iv. editProjectOrTask() --> Lets user edit a project or task based on id input

    */





    /*--------------------------------------Text Prompts and Menu Traversal--------------------------------------*/

    void terminalMenu(string type)
    {
        if (type == "main")
        {
            UtilsText.ShowMenu(type);

            int choice;
            choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    UtilsText.ClearScreen();
                    displayProjectsAndTasks();
                    break;
                case 2:
                    UtilsText.ClearScreen();
                    createATask();
                    terminalMenu("main");
                    break;
                case 3:
                    UtilsText.ClearScreen();
                    createAProject();
                    terminalMenu("main");
                    break;
                case 4:
                    UtilsText.ClearScreen();
                    TwoWeeks.printNDaysFromToday(tasks, projects, 14);
                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    terminalMenu("main");
                    break;
                case 5:
                    UtilsText.ClearScreen();
                    SubToolManager.Run();
                    break;
                default:
                    UtilsText.ClearScreen();
                    UtilsText.Greetings();
                    terminalMenu("main");
                    break;
            }
        }
        else if (type == "viewandedit")
        {
            UtilsText.ShowMenu(type);
            int choice;
            choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case -1:
                    UtilsText.ClearScreen();
                    UtilsText.Greetings();
                    terminalMenu("main");
                    break;
                case -2:
                    UtilsText.ClearScreen();
                    createATask();
                    terminalMenu("main");
                    break;
                case -3:
                    UtilsText.ClearScreen();
                    createAProject();
                    terminalMenu("main");
                    break;
                default:
                    UtilsText.ClearScreen();
                    editProjectOrTask(choice, "none");
                    break;
            }
        }
    }

    void displayProjectsAndTasks()
    {
        UtilsText.ClearScreen();
        Console.WriteLine("\t\t --- Projects and Tasks ---\n");

        string projectsAndTasksLogs = getProjectsAndTasksLogs();    
        Console.WriteLine(projectsAndTasksLogs);       

        terminalMenu("viewandedit");
    }
    /*-----------------------------------------------------------------------------------------------------------*/



    string GetProjectsAndTasksLogPath()
    {
        return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "logs", "projectsAndTasksLogs.txt"));
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
                    break;
                }
            }
        }

        tasks.Add(new UtilsTask(freshId("task"), taskName, isImportant, isUrgent, dueDate, estimatedHours, projectId));
        saveChanges();
        terminalMenu("main");
    }

    void editProjectOrTask(int id, string taskOrProj)
    {
        int projectId = -1;
        int taskId = -1;
        if (taskOrProj == "none")
        {
            foreach (UtilsProject project in projects)
            {
                if (id == project.GetProjectId())
                {
                    projectId = project.GetProjectId();
                    break;
                }
            }
            foreach (UtilsTask task in tasks)
            {
                if (id == task.GetTaskId())
                {
                    taskId = task.GetTaskId();
                    break;
                }
            }
            if (projectId != -1 && taskId != -1)
            {
                UtilsProject projectToEdit = null;
                foreach (UtilsProject project in projects)
                {
                    if (id == project.GetProjectId())
                    {
                        projectToEdit = project;
                        break;
                    }
                }
                UtilsTask taskToEdit = null;
                foreach (UtilsTask task in tasks)
                {
                    if (id == task.GetTaskId())
                    {
                        taskToEdit = task;
                        break;
                    }
                }
                UtilsText.EditATaskOrProject("both", projectToEdit, taskToEdit, "none");
                int choice = int.Parse(Console.ReadLine());
                if (choice == 1)
                {
                    taskOrProj = "proj";
                }
                else if (choice == 2)
                {
                    taskOrProj = "task";
                }
                else if (choice == 3)
                {
                    displayProjectsAndTasks();
                }
            }
        }
        if (taskOrProj == "none")
        {
            if (taskId != -1)
            {
                taskOrProj = "task";
            }
            else if (projectId != -1)
            {
                taskOrProj = "proj";
            }
        }

        if (taskOrProj == "proj")
        {
            projectId = id;
            UtilsProject projectToEdit = null;
            foreach (UtilsProject project in projects)
            {
                if (projectId == project.GetProjectId())
                {
                    projectToEdit = project;
                    break;
                }
            }
            UtilsText.EditATaskOrProject("project", projectToEdit, null, "none");
            int choice = int.Parse(Console.ReadLine());
            if (choice == 1)
            {
                UtilsText.CreateATaskOrProject("pname");
                string name = Console.ReadLine();
                projectToEdit.updateName(name);
                saveChanges();
                editProjectOrTask(projectId, taskOrProj);
            }
            else if (choice == 2)
            {
                UtilsText.CreateATaskOrProject("pimportant");
                bool important = Console.ReadLine().ToLower() == "y";
                projectToEdit.updateIsImportant(important);
                saveChanges();
                editProjectOrTask(projectId, taskOrProj);
            }
            else if (choice == 3)
            {
                UtilsText.CreateATaskOrProject("purgent");
                bool urgent = Console.ReadLine().ToLower() == "y";
                projectToEdit.updateIsUrgent(urgent);
                saveChanges();
                editProjectOrTask(projectId, taskOrProj);
            }
            else if (choice == 4)
            {
                UtilsText.CreateATaskOrProject("pdue");
                DateTime due = DateTime.Parse(Console.ReadLine());
                projectToEdit.updateDueDate(due);
                saveChanges();
                editProjectOrTask(projectId, taskOrProj);
            }
            else if (choice == 5)
            {
                UtilsText.CreateATaskOrProject("phours");
                float hours = float.Parse(Console.ReadLine());
                projectToEdit.updateEstimatedHours(hours);
                saveChanges();
                editProjectOrTask(projectId, taskOrProj);
            }
            else if (choice == 6)
            {
                UtilsText.EditATaskOrProject("deleteProject", projectToEdit, null, "none");
                bool deleteAllSubTasks = Console.ReadLine() == "y";
                if (deleteAllSubTasks)
                {
                    List<UtilsTask> markedForDeletion = new List<UtilsTask>();
                    foreach (UtilsTask task in tasks)
                    {
                        if (task.getProjectId() == projectToEdit.GetProjectId())
                        {
                            markedForDeletion.Add(task);
                        }
                    }
                    for (int i = 0; i < markedForDeletion.Count; i++)
                    {
                        tasks.Remove(markedForDeletion.ElementAt(i));
                    }
                }
                else
                {
                    foreach (UtilsTask task in tasks)
                    {
                        if (task.getProjectId() == projectToEdit.GetProjectId())
                        {
                            task.updateProjectId(-1);
                        }
                    }
                }
                projects.Remove(projectToEdit);
                saveChanges();
                displayProjectsAndTasks();
            }
            else if (choice == 7)
            {
                displayProjectsAndTasks();
            }
        }
        else if (taskOrProj == "task")
        {
            taskId = id;
            UtilsTask taskToEdit = null;
            foreach (UtilsTask task in tasks)
            {
                if (taskId == task.GetTaskId())
                {
                    taskToEdit = task;
                    break;
                }
            }
            string projectName = "this task is not a part of a project";
            foreach (UtilsProject project in projects)
            {
                if (project.GetProjectId() == taskToEdit.getProjectId())
                {
                    projectName = project.GetProjectName();
                    break;
                }
            }
            UtilsText.EditATaskOrProject("task", null, taskToEdit, projectName);
            int choice = int.Parse(Console.ReadLine());
            if (choice == 1)
            {
                UtilsText.CreateATaskOrProject("tname");
                string name = Console.ReadLine();
                taskToEdit.updateTaskName(name);
                saveChanges();
                editProjectOrTask(taskId, taskOrProj);
            }
            else if (choice == 2)
            {
                UtilsText.CreateATaskOrProject("timportant");
                bool important = Console.ReadLine().ToLower() == "y";
                taskToEdit.updateIsImportant(important);
                saveChanges();
                editProjectOrTask(taskId, taskOrProj);
            }
            else if (choice == 3)
            {
                UtilsText.CreateATaskOrProject("turgent");
                bool urgent = Console.ReadLine().ToLower() == "y";
                taskToEdit.updateIsUrgent(urgent);
                saveChanges();
                editProjectOrTask(taskId, taskOrProj);
            }
            else if (choice == 4)
            {
                UtilsText.CreateATaskOrProject("tdue");
                DateTime due = DateTime.Parse(Console.ReadLine());
                taskToEdit.updateDueDate(due);
                saveChanges();
                editProjectOrTask(taskId, taskOrProj);
            }
            else if (choice == 5)
            {
                UtilsText.CreateATaskOrProject("thours");
                float hours = float.Parse(Console.ReadLine());
                taskToEdit.updateEstimatedHours(hours);
                saveChanges();
                editProjectOrTask(taskId, taskOrProj);
            }
            else if (choice == 6)
            {
                UtilsText.CreateATaskOrProject("tprojectname");
                string projectname = Console.ReadLine();
                foreach (UtilsProject project in projects)
                {
                    if (projectname == project.GetProjectName())
                    {
                        taskToEdit.updateProjectId(project.GetProjectId());
                        break;
                    }
                }
                saveChanges();
                editProjectOrTask(taskId, taskOrProj);
            }
            else if (choice == 7)
            {
                tasks.Remove(taskToEdit);
                saveChanges();
                displayProjectsAndTasks();
            }
            else if (choice == 8)
            {
                displayProjectsAndTasks();
            }
        }
    }

    int freshId(string type)
    {
        int id = 1;
        if (type == "project")
        {
            bool freshId = false;
            while (!freshId)
            {
                freshId = true;
                foreach (UtilsProject project in projects)
                {
                    if (id == project.GetProjectId())
                    {
                        freshId = false;
                    }
                }
                if (freshId)
                {
                    return id;
                }
                else
                {
                    id++;
                }
            }
        }
        else
        {
            bool freshId = false;
            while (!freshId)
            {
                freshId = true;
                foreach (UtilsTask t in tasks)
                {
                    if (id == t.GetTaskId())
                    {
                        freshId = false;
                        break;
                    }
                }
                if (freshId)
                {
                    return id;
                }
                else
                {
                    id++;
                }
            }
        }
        return -1;
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


        projects.Add(new UtilsProject(freshId("project"), name, isImportant, isUrgent, dueDate, estimatedHours));
        saveChanges();
        terminalMenu("main");
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

    public void Run()
    {
        loadData();
        UtilsText.Greetings();
        terminalMenu("main");
    }
    }
}
