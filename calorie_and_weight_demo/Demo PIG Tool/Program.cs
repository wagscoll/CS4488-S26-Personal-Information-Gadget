
// Calorie Tracker and Weight Tracker Demo
global using HealthLog = (int date, float weight, float calories);

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;

List<HealthLog> healthLogs = new();

/*
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




/*--------------------------------------Text Prompts and Menu Traversal--------------------------------------*/

void greetings()
{
    Console.WriteLine("Hello! This is a Health Tracking Tool.");
    Console.WriteLine("You can log your daily calories, weight, and analyze your health data.\n");
    terminalMenu();
}

void terminalMenu()
{
    int choice1;
    Console.WriteLine("\t--- Menu ---");
    Console.WriteLine("Press 1 to view your 'Health Logs'");
    Console.WriteLine("Press 2 to input new today's data");

    choice1 = int.Parse(Console.ReadLine());

    if (choice1 == 1)
    {
        clearCLIScreen();
        Console.WriteLine("Health Logs - Calories and Weight Tracking");
        displayHealthLogs();
    }
    else if (choice1 == 2)
    {
        clearCLIScreen();
        inputNewHealthData();
    }
}

void displayHealthLogs()
{
    clearCLIScreen();

    Console.WriteLine("\t\t --- Health Logs ---\n");

    /*      -- Covert to struct or tuple for better data handling later --
    foreach (var dateLog in healthLogs)
    {
        Console.WriteLine("Date: " + dateLog.date +
            "    \t| Weight: " + dateLog.weight +
            "    \t| Calories: " + dateLog.calories);
    }
    */

    string healthLogs = getHealthLogs();    // populate healthLogs list
    Console.WriteLine(healthLogs);          // display healthLogs data

    Console.WriteLine("\n\n --- Menu --- ");
    Console.WriteLine("Press 1 to modify logs");
    Console.WriteLine("Press 2 to return to main menu");

    int choice2 = int.Parse(Console.ReadLine());
    if (choice2 == 2)
    {
        clearCLIScreen();
        terminalMenu();
    }

    else if(choice2 == 1)
    {
        clearCLIScreen();
        Console.WriteLine("Modify Logs - Feature Coming Soon!");
        Console.WriteLine("Press any key to return to main menu...");
        Console.ReadKey();
        clearCLIScreen();
        terminalMenu();
    }
}
/*-----------------------------------------------------------------------------------------------------------*/



string GetProjectHealthLogPath()
{
    return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "healthlogs.txt"));
}

string getHealthLogs()
{
    string path = GetProjectHealthLogPath();
    if (File.Exists(path))
    {
        string readText = File.ReadAllText(path);
        Console.WriteLine(readText);
        return readText;
    }

    Console.WriteLine("Log file not found.");
    return "";
}

void clearCLIScreen()
{
    Console.Clear();
}



//---------------------------------------------Logging Functions---------------------------------------------
string logDate()
{
    DateTime now = DateTime.Now;
    string currentDate = now.ToString("yyyy-MM-dd");
    return currentDate;
}

float logWeight()
{
    Console.Write("Weight (lbs): ");
    float currentWeight = float.Parse(Console.ReadLine());
    return currentWeight;
}

float logCalories()
{
    Console.Write("Calories Burned: ");
    float caloriesBurned = float.Parse(Console.ReadLine());
    return caloriesBurned;
}
/*---------------------------------------------------------------------------------------------------------*/



//---------------------------------------------Path & .txt Functions---------------------------------------------

void inputNewHealthData()
{
    //validate input data here!!!

    Console.WriteLine("Log Today's Health Data");
    Console.WriteLine("Today's Date: " + logDate());

    float currentWeight = logWeight();
    float caloriesConsumed = logCalories();

    //store data to file healthlogs.txt
    submitHealthData(logDate(), currentWeight, caloriesConsumed);
 
    Console.WriteLine("New health data logged successfully!");
}

void submitHealthData(string date, float weight, float calories)
{
    string path = GetProjectHealthLogPath();
    using (StreamWriter sw = File.AppendText(path))
    {
        sw.WriteLine(date + "	     " + weight + "       	" + calories);
    }
}
/*---------------------------------------------------------------------------------------------------------*/

Console.WriteLine("Welcome to the Calorie and Weight Tracker!");
greetings();




    