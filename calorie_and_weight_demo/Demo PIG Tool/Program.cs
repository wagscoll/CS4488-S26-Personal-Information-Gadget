
// Calorie Tracker and Weight Tracker Demo
global using HealthLog = (int date, float weight, float calories);
using System;
using System.IO;
using System.Collections.Generic;
using Demo_PIG_Tool.Utils;




List<HealthLog> healthLogs = new();

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
            displayHealthLogs();
            break;
        case 2:
            UtilsText.ClearScreen();
            inputNewHealthData();
            break;
        case 3:
            UtilsText.ClearScreen();
            UtilsText.FeatureComingSoon();
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

void displayHealthLogs()
{
    UtilsText.ClearScreen();
    Console.WriteLine("\t\t --- Health Logs ---\n");

    string healthLogs = getHealthLogs();    // populate healthLogs list
    Console.WriteLine(healthLogs);          // display healthLogs data

    terminalMenu();
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


//---------------------------------------------Logging Functions---------------------------------------------
float getWeight()    //validate input data here!!!
{
    UtilsText.LogHealthData(2);
    float currentWeight = float.Parse(Console.ReadLine());
    return currentWeight;
}

float getCalories()    //validate input data here!!!
{
    UtilsText.LogHealthData(3);
    float caloriesConsumed = float.Parse(Console.ReadLine());
    return caloriesConsumed;
}
/*---------------------------------------------------------------------------------------------------------*/



//---------------------------------------------Path & .txt Functions---------------------------------------------
void inputNewHealthData()
{
    UtilsText.LogHealthData(1);

    float currentWeight = getWeight();
    float caloriesConsumed = getCalories();

    submitHealthData(UtilsDate.GetDate(), currentWeight, caloriesConsumed);     //store data to file healthlogs.txt
 
    UtilsText.LogHealthData(4);
    terminalMenu();
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




UtilsText.Greetings();
terminalMenu();


    