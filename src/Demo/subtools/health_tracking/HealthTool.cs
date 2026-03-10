
// Calorie Tracker and Weight Tracker Demo
global using HealthLog = (int date, float weight, float calories);

using System;
using System.IO;
using Demo_PIG_Tool.Utils;
using Demo_PIG_Tool.Manager;

namespace Demo_PIG_Tool.HealthTool
{
    public static class HealthTool
    {
        public static void Run()
        {
            new Program().Run();

        }
    
        /*  1/28/26 - Collin

        Work Flow:
            1. Run()
            2. Program.Run()
            3. Greetings()
            4. terminalMenu()
            5. Options:
                A. displayHealthLogs()
                    i. GetProjectHealthLogPath() -> resolves /logs/healthlogs.txt
                    ii. getHealthLogs() -> reads entire healthlogs.txt (or prints “not found”)
                    iii. print logs -> return to terminalMenu()
                B. inputNewHealthData()
                    i. collect data via:
                        a. UtilsDate.GetDate()
                        b. getWeight() -> prompt + validate (repeat until valid)
                        c. getCalories() -> prompt + validate (repeat until valid)
                            ii. submitHealthData() -> appends one line to healthlogs.txt (format: date | weight | calories)
                            iii. confirmation -> return to terminalMenu()
                C. FeatureComingSoon() -> return to terminalMenu()
                D. Greetings() -> return to terminalMenu()
                E. SubToolManager.Run() (exit health tool)

        */



        class Program
        {
            /*--------------------------------------Text Prompts and Menu Traversal--------------------------------------*/

            void terminalMenu()
            {   
                HealthText.ShowMenu();

                int choice1;
                choice1 = int.Parse(Console.ReadLine());

                switch (choice1)
                {
                    case 1:
                        HealthText.ClearScreen();
                        displayHealthLogs();
                        break;
                    case 2:
                        HealthText.ClearScreen();
                        inputNewHealthData();
                        break;
                    case 3:
                        HealthText.ClearScreen();
                        HealthText.FeatureComingSoon(); //////////////////////////////////////////////////// do this
                        terminalMenu();
                        break;
                    case 4:
                        HealthText.ClearScreen();
                        HealthText.Greetings();
                        terminalMenu();
                        break;
                    case 5:
                        SubToolManager.Run();
                        break;
                    default:
                        HealthText.ClearScreen();
                        HealthText.Greetings();
                        terminalMenu();
                        break;
                }
            }

            void displayHealthLogs()
            {
                HealthText.ClearScreen();
                Console.WriteLine("\t\t --- Health Logs ---\n");

                string healthLogs = getHealthLogs();    // populate healthLogs list
                Console.WriteLine(healthLogs);          // display healthLogs data

                terminalMenu();
            }
            /*-----------------------------------------------------------------------------------------------------------*/


            /*                          ".."                    ".."             ".."         ".."                                             
            current file HealthTool.cs ------> Health_tracking ------> subtools ------> Demo ------> src --> logs --> healthlogs.txt
            */
            string GetProjectHealthLogPath()
            {
                return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "logs", "healthlogs.txt"));
            }

            string getHealthLogs()
            {
                string path = GetProjectHealthLogPath();
                if (File.Exists(path))
                {
                    string readText = File.ReadAllText(path);
                    return readText;
                }

                Console.WriteLine("Log file not found.");
                return "";
            }



            //---------------------------------------------Logging Functions---------------------------------------------
            float getWeight() 
            {
                HealthText.LogHealthData(2);
                float currentWeight = float.Parse(Console.ReadLine());

                if(!inputValidation(currentWeight))
                {
                    Console.WriteLine("Invalid input. Please enter a valid number for weight.");
                    return getWeight();
                }
                return currentWeight;
            }

            float getCalories()   
            {
                HealthText.LogHealthData(3);
                float caloriesConsumed = float.Parse(Console.ReadLine());

                if(!inputValidation(caloriesConsumed))
                {
                    Console.WriteLine("Invalid input. Please enter a valid number for calories consumed.");
                    return getCalories();
                }
                
                return caloriesConsumed;
            }
            /*---------------------------------------------------------------------------------------------------------*/

            bool inputValidation(float value)
            {

                if(value < 0 || value > 100000)
                {
                    return false;
                }

                else if(float.IsNaN(value))
                {
                    return false;
                }

                return true;
            }


            //---------------------------------------------Path & .txt Functions---------------------------------------------
            void inputNewHealthData()
            {
                HealthText.LogHealthData(1);

                float currentWeight = getWeight();
                float caloriesConsumed = getCalories();

                submitHealthData(UtilsDate.GetDate(), currentWeight, caloriesConsumed);     //store data to file healthlogs.txt
            
                HealthText.LogHealthData(4);
                terminalMenu();
            }


            //This function adds new health data to the healthlogs.txt file in the format: date | weight | calories
            //For data parsing, we can split the string by " | " to retrieve individual data points
            void submitHealthData(string date, float weight, float calories)        //update this formatting
            {
                string path = GetProjectHealthLogPath();
                using (StreamWriter sw = File.AppendText(path))
                {
                    string healthData = string.Join(" | ", date, weight, calories);
                    sw.WriteLine(healthData);
                }
            }
            /*---------------------------------------------------------------------------------------------------------*/


            public void Run()
            {
                HealthText.Greetings();
                terminalMenu();
            }
        }
    }
}



    