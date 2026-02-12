using System;
using System.Collections.Generic;
namespace Demo_PIG_Tool.BudgetTool;

/* Author: Gabriel Ory 
 * This class provides a simple demo of the Budget Expenses sub-tool.
 * It allows the user to create budgets, add expenses, and view the total expenses and remaining budget for each category.
 */


    public class BudgetDemo
    {
        // runs the demo, allowing the user to create budgets, add expenses, and view the total expenses and remaining budget for each category.
        public static void Run()
        {
            Console.WriteLine("Welcome to the Budget Demo! \n");
            Console.WriteLine("NOTE FROM DEVELOPER: Input validation has not been added to this demo yet. In the meantime, please follow the requested input format for each prompt.\n");

            List<Budget> allBudgets = new List<Budget>();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Choose an option: (1-4)");
                Console.WriteLine("1. Create a new budget");
                Console.WriteLine("2. Add an expense");
                Console.WriteLine("3. View total expenses and remaining budget");
                Console.WriteLine("4. Create a shopping list (coming soon)");
                Console.WriteLine("5. Exit \n");

                int option = int.Parse(Console.ReadLine());

                // create a new budget 
                if (option == 1)
                {
                    Console.WriteLine("\nCreating a new budget... \n");

                    // get user input for month, year, and monthly income
                    Console.WriteLine("Enter the month (1-12):");
                    int month = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter the year (YYYY):");
                    int year = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter your monthly income:");
                    decimal income = decimal.Parse(Console.ReadLine());

                    // create a new budget with the provided information
                    Budget newBudget = new Budget(month, year, income);

                    // get user input for budget categories and amounts
                    Console.WriteLine("\nEnter your budget for each category:");
                    Console.WriteLine("Savings:");
                    decimal savingsBudget = decimal.Parse(Console.ReadLine());
                    Console.WriteLine("Insurance:");
                    decimal insuranceBudget = decimal.Parse(Console.ReadLine());
                    Console.WriteLine("Rent/Mortage:");
                    decimal rentBudget = decimal.Parse(Console.ReadLine());
                    Console.WriteLine("Gas:");
                    decimal gasBudget = decimal.Parse(Console.ReadLine());
                    Console.WriteLine("Food:");
                    decimal foodBudget = decimal.Parse(Console.ReadLine());
                    Console.WriteLine("Fun:");
                    decimal funBudget = decimal.Parse(Console.ReadLine());
                    Console.WriteLine("Other:");
                    decimal otherBudget = decimal.Parse(Console.ReadLine());

                    // add category budgets to the budget
                    newBudget.Categories[0].CategoryBudget = savingsBudget;
                    newBudget.Categories[1].CategoryBudget = insuranceBudget;
                    newBudget.Categories[2].CategoryBudget = rentBudget;
                    newBudget.Categories[3].CategoryBudget = gasBudget;
                    newBudget.Categories[4].CategoryBudget = foodBudget;
                    newBudget.Categories[5].CategoryBudget = funBudget;
                    newBudget.Categories[6].CategoryBudget = otherBudget;

                    allBudgets.Add(newBudget);

                    Console.WriteLine("New budget created. \n");
                }
                // add an expense to a budget
                else if (option == 2)
                {
                    Console.WriteLine("\nAdding an expense...\n");

                    if (allBudgets.Count == 0)
                    {
                        Console.WriteLine("You must create a budget first. \n");
                    }
                    else
                    {
                        Console.WriteLine("Select the budget (month/year) to which you want to add an expense:");

                        for (int i = 1; i < allBudgets.Count + 1; i++)
                        {
                            Console.WriteLine(i + ": " + allBudgets[i - 1].Month + "/" + allBudgets[i - 1].Year);
                        }

                        Console.WriteLine("");
                        int budgetIndex = int.Parse(Console.ReadLine());
                        Budget budget = allBudgets[budgetIndex - 1];

                        Console.WriteLine("\nEnter the date of the expense (YYYY-MM-DD):");
                        DateTime date = DateTime.Parse(Console.ReadLine());
                        Console.WriteLine("Enter a description for the expense:");
                        string description = Console.ReadLine();
                        Console.WriteLine("Enter the amount of the expense:");
                        decimal amount = decimal.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the category for the expense: (Categories: Savings, Insurance, Rent/Mortage, Gas, Food, Fun, Other." +
                            " If the entered category does not match one of the default categories, the expense is added to Other.)");
                        string categoryName = Console.ReadLine();

                        budget.AddExpense(new Expense(date, description, amount, categoryName));

                        Console.WriteLine("The expense was added. \n");
                    }
                }
                // view total expenses and remaining budget for each category
                else if (option == 3)
                {
                    Console.WriteLine("\nViewing total expenses and remaining budget...\n");

                    if (allBudgets.Count == 0)
                    {
                        Console.WriteLine("You must create a budget first.\n");
                    }
                    else
                    {
                        Console.WriteLine("Select the budget (month/year) to which you want to add an expense:");
                        for (int i = 1; i < allBudgets.Count + 1; i++)
                        {
                            Console.WriteLine(i + ": " + allBudgets[i - 1].Month + "/" + allBudgets[i - 1].Year);
                        }

                        Console.WriteLine("");
                        int budgetIndex = int.Parse(Console.ReadLine());
                        Budget budget = allBudgets[budgetIndex - 1];
                        BudgetPrinter.Print(budget);
                        Console.WriteLine("");
                    }
                }
                // create a shopping list 
                else if (option == 4)
                {
                    Console.WriteLine("\nThe shopping list feature is coming soon! \n");
                }
                // exit the demo
                else if (option == 5)
                {
                    Console.WriteLine("\nThank you for trying this demo.");
                    exit = true;
                }
                else
                {
                    Console.WriteLine("\nInvalid input.");
                }
            }
        }
    }

