
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
    
    
        /* Author: Gabriel Ory*/
        //BudgetDemo budgetDemo = new BudgetDemo();
        //BudgetDemo.Run();

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
                        HealthText.FeatureComingSoon();
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
                    return readText;
                }

                Console.WriteLine("Log file not found.");
                return "";
            }



            //---------------------------------------------Logging Functions---------------------------------------------
            float getWeight()    //validate input data here!!!
            {
                HealthText.LogHealthData(2);
                float currentWeight = float.Parse(Console.ReadLine());
                return currentWeight;
            }

            float getCalories()    //validate input data here!!!
            {
                HealthText.LogHealthData(3);
                float caloriesConsumed = float.Parse(Console.ReadLine());
                return caloriesConsumed;
            }
            /*---------------------------------------------------------------------------------------------------------*/



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

            void submitHealthData(string date, float weight, float calories)        //update this formatting
            {
                string path = GetProjectHealthLogPath();
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(date + "	     " + weight + "       	" + calories);
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



    