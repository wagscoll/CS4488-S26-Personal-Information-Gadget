using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo_PIG_Tool.Utils
{
    public static class TwoWeeks
    {
        // Displays tasks and projects due in the next n days (default 14)
        public static void printNDaysFromToday(List<UtilsTask> tasks, List<UtilsProject> projects, int n = 14)
        {
            // Get today's date and calculate the end date
            // We add n+1 days to include tasks and projects due on the nth day
            DateTime today = DateTime.Today;
            DateTime endDate = today.AddDays(n + 1);

            // Filter tasks and projects that are due between today and the end date    
            // We use >= today to include tasks/projects due today, 
            // and < endDate to exclude those due on the day after the nth day
            var tasksDueInTwoWeeks = tasks
                .Where(task => task.getDueDate() >= today && task.getDueDate() < endDate)
                .ToList();

            // Group tasks and projects by their due date for easier display
            var projectsDueInTwoWeeks = projects
                .Where(project => project.getDueDate() >= today && project.getDueDate() < endDate)
                .ToList();

            // Group projects by their due date
            var groupedProjects = projectsDueInTwoWeeks
                .GroupBy(project => project.getDueDate().Date)
                .ToDictionary(g => g.Key, g => g.ToList());


            Console.WriteLine($"Tasks due in the next {n} days:\n");

            // Group tasks by their due date, aswell
            // Store in a dictionary for easy lookup when displaying
            var groupedTasksDict = tasksDueInTwoWeeks
                .GroupBy(task => task.getDueDate().Date)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Get a sorted list of all unique due dates from both tasks and projects
            // We use Union to combine the keys from both dictionaries, and OrderBy to sort them
            var allDates = groupedTasksDict.Keys
                .Union(groupedProjects.Keys)
                .OrderBy(d => d)
                .ToList();

            // If there are no tasks or projects due in the next n days, inform the user and return!
            if (allDates.Count == 0)
            {
                Console.WriteLine("(No tasks or projects due in this range.)\n");
                return;
            }

            int dayCounter = 1;
            // Iterate through each date and display the tasks and projects due on that date
            foreach (var date in allDates)
            {
                Console.WriteLine($"{dayCounter}) {date:ddd MMM dd, yyyy}");

                // First display tasks due on that date, if any (we check the dictionary we built earlier for easy lookup)
                if (groupedTasksDict.TryGetValue(date, out var tasksOnThatDay))
                {
                    //Sort tasks by due time, then by urgency, importance, and finally alphabetically by name
                    var sortedTasks = tasksOnThatDay
                        .OrderBy(t => t.getDueDate().TimeOfDay)
                        .ThenByDescending(t => t.getisUrgent())
                        .ThenByDescending(t => t.getisImportant())
                        .ThenBy(t => t.GetTaskName())
                        .ToList();

                    // Display tasks with a letter label (a, b, c, etc.)       
                    char letter = 'a';
                    // For each task, we display the due time, name, project (if any), and tags (Urgent/Important)
                    foreach (var sortedTask in sortedTasks)
                    {
                        // Format the due time as "hh:mm tt" (e.g., "02:30 PM")
                        string time = sortedTask.getDueDate().ToString("hh:mm tt");
                        string tags = BuildTags(sortedTask);
                        string proj = ProjectLabel(sortedTask, projects);

                        // Display the task with its letter label, due time, name, project label, and tags
                        Console.WriteLine($"\t{letter}) {time} {sortedTask.GetTaskName()}{proj}{tags}");
                        // Increment the letter for the next task
                        letter++;
                    }
                }

                // Then display projects due on that date, if any (we check the groupedProjects dictionary for easy lookup)
                if (groupedProjects.TryGetValue(date, out var projectsOnThatDay))
                {
                    Console.WriteLine("\t-- Projects due --");
                    foreach (var project in projectsOnThatDay
                    // We sort projects by due time, then by urgency, importance, and finally alphabetically by name
                        .OrderBy(p => p.getDueDate().TimeOfDay)
                        .ThenByDescending(p => p.getisUrgent())
                        .ThenByDescending(p => p.getisImportant())
                        .ThenBy(p => p.GetProjectName()))
                    {
                        string ptime = project.getDueDate().ToString("hh:mm tt");
                        string ptags = BuildTags(project);
                        Console.WriteLine($"\t   • {ptime} {project.GetProjectName()}{ptags}");
                    }
                }

                // Add an extra line break after each day for better readability
                Console.WriteLine();
                dayCounter++;
            }
        }

        //Below are helper methods to build the tags string for tasks and projects based on their urgency and importance.

        // Helper method to build the tags string for a task based on its urgency and importance
        private static string BuildTags(UtilsTask task)
        {
            List<string> tags = new();
            if (task.getisUrgent()) tags.Add("Urgent");
            if (task.getisImportant()) tags.Add("Important");
            return tags.Count > 0 ? $" [{string.Join(", ", tags)}]" : "";
        }

        // Overloaded helper method to build the tags string for a project based on its urgency and importance

        private static string BuildTags(UtilsProject project)
        {
            List<string> tags = new();
            if (project.getisUrgent()) tags.Add("Urgent");
            if (project.getisImportant()) tags.Add("Important");
            return tags.Count > 0 ? $" [{string.Join(", ", tags)}]" : "";
        }

        // Helper method to get the project label for a task, if it belongs to a project
        private static string ProjectLabel(UtilsTask task, List<UtilsProject> projects)
        {
            int pid = task.getProjectId();
            if (pid == -1) return "";

            var p = projects.FirstOrDefault(x => x.GetProjectId() == pid);
            return p == null ? " [Project: Unknown]" : $" [Project: {p.GetProjectName()}]";
        }
    }
}
