using System;
using System.Collections.Generic;
namespace Demo_PIG_Tool.BudgetTool;

/* Author: Gabriel Ory 
 * This class provides a simple demo of the Budget Expenses sub-tool.
 * It allows the user to create budgets, add expenses, and view the total expenses and remaining budget for each category.
 * It also allows the user to create shopping lists, add grocery items to the shopping lists, and edit the grocery items in the shopping lists.
 */


    public class BudgetDemo
    {
        // runs the demo
        public static void Run()
        {
            Console.WriteLine("Welcome to the Budget Demo! \n");
            Console.WriteLine("NOTE FROM DEVELOPER: Input validation has not been added to this demo yet. In the meantime, please follow the requested input format for each prompt.\n");

            List<Budget> allBudgets = new List<Budget>();
            List<ShoppingList> allShoppingLists = new List<ShoppingList>();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Choose an option: (1-4)");
                Console.WriteLine("1. Create a new budget");
                Console.WriteLine("2. Add an expense");
                Console.WriteLine("3. View total expenses and remaining budget");
                Console.WriteLine("4. Create a shopping list");
                Console.WriteLine("5. Edit a shopping list");
                Console.WriteLine("6. View shopping list");
                Console.WriteLine("7. Exit \n");

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
                    
                    // if there are no budgets, prompt the user to create a budget first
                    if (allBudgets.Count == 0)
                    {
                        Console.WriteLine("You must create a budget first. \n");
                    }
                    // if there are budgets, prompt the user to select a budget and enter expense information
                    else
                    {
                        Console.WriteLine("Select the budget (month/year) to which you want to add an expense:");

                        // display the budgets with their corresponding indices for selection
                        for (int i = 1; i < allBudgets.Count + 1; i++)
                        {
                            Console.WriteLine(i + ": " + allBudgets[i - 1].Month + "/" + allBudgets[i - 1].Year);
                        }

                        Console.WriteLine("");
                        int budgetIndex = int.Parse(Console.ReadLine());
                        Budget budget = allBudgets[budgetIndex - 1];

                        // get user input for expense information
                        Console.WriteLine("\nEnter the date of the expense (YYYY-MM-DD):");
                        DateTime date = DateTime.Parse(Console.ReadLine());
                        Console.WriteLine("Enter a description for the expense:");
                        string description = Console.ReadLine();
                        Console.WriteLine("Enter the amount of the expense:");
                        decimal amount = decimal.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the category for the expense: (Categories: Savings, Insurance, Rent/Mortage, Gas, Food, Fun, Other." +
                            " If the entered category does not match one of the default categories, the expense is added to Other.)");
                        string categoryName = Console.ReadLine();

                        // add the expense to the selected budget
                        budget.AddExpense(new Expense(date, description, amount, categoryName));
                        Console.WriteLine("The expense was added. \n");
                    }
                }
                // view total expenses and remaining budget for each category
                else if (option == 3)
                {
                    Console.WriteLine("\nViewing total expenses and remaining budget...\n");

                    // if there are no budgets, prompt the user to create a budget first
                    if (allBudgets.Count == 0)
                    {
                        Console.WriteLine("You must create a budget first.\n");
                    }
                    // if there are budgets, prompt the user to select a budget to view
                    else
                    {
                        Console.WriteLine("Select the budget (month/year) you want to view:");

                        // display the budgets with their corresponding indices for selection
                        for (int i = 1; i < allBudgets.Count + 1; i++)
                        {
                            Console.WriteLine(i + ": " + allBudgets[i - 1].Month + "/" + allBudgets[i - 1].Year);
                        }

                        Console.WriteLine("");
                        int budgetIndex = int.Parse(Console.ReadLine());

                        // print the total expenses and remaining budget for each category of the selected budget
                        Budget budget = allBudgets[budgetIndex - 1];
                        BudgetPrinter.Print(budget);
                        Console.WriteLine("");
                    }
                }
                // create a shopping list 
                else if (option == 4)
                {
                    Console.WriteLine("Creating a shopping list...\n");
                    Console.WriteLine("Enter the name of the shopping list:");
                    string listName = Console.ReadLine();

                    ShoppingList shoppingList = new ShoppingList(listName, new List<Grocery>());

                    bool keepAdding = true;
                    while (keepAdding)
                    {
                        Console.WriteLine("\nDo you want to add an item to the shopping list? (y/n)");
                        string addNow = Console.ReadLine().ToLower();
                        if (addNow == "y")
                        {   
                            // get user input for grocery item information
                            Console.WriteLine("\nEnter the name of the new item:");
                            string name = Console.ReadLine();
                            Console.WriteLine("Enter the best store to buy the item:");
                            string bestStore = Console.ReadLine();
                            Console.WriteLine("Enter the price of the item:");
                            decimal price = decimal.Parse(Console.ReadLine());
                            Console.WriteLine("Enter the quantity of the item:");
                            int quantity = int.Parse(Console.ReadLine());
                            
                            // add the grocery item to the shopping list
                            shoppingList.AddItem(new Grocery(name, bestStore, price, quantity));
                        }
                        else if (addNow == "n")
                        {
                            Console.WriteLine("\nYou can add items later. Exiting to option menu.\n");
                            keepAdding = false;
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid input. Please enter 'y' or 'n'.");
                        }
                    }
                 
                    allShoppingLists.Add(shoppingList);
                    Console.WriteLine("Shopping list '" + listName + "' created.\n");
                }
                // edit a shopping list
                else if (option == 5)
                {
                    Console.WriteLine("\nEditing a shopping list...\n");
                    Console.WriteLine("Select the shopping list to edit:");

                    // if there are no shopping lists, prompt the user to create a shopping list first
                    if (allShoppingLists.Count == 0)
                    {
                        Console.WriteLine("You must create a shopping list first.\n");
                        continue;
                    }

                    // display the shopping lists with their corresponding indices for selection
                    for (int i = 1; i < allShoppingLists.Count + 1; i++)
                    {
                        Console.WriteLine(i + ": " + allShoppingLists[i - 1].Name);
                    }

                    // get the selected shopping list to edit
                    Console.WriteLine("");
                    int listIndex = int.Parse(Console.ReadLine());
                    ShoppingList shoppingList = allShoppingLists[listIndex - 1];

                    bool keepEditing = true;

                    while (keepEditing)
                    {
                        Console.WriteLine("\nChoose an option:");
                        Console.WriteLine("1. Add an item");
                        Console.WriteLine("2. Remove an item");
                        Console.WriteLine("3. Change quantity of an item");
                        Console.WriteLine("4. Change price of an item");
                        Console.WriteLine("5. Change best store of an item");
                        Console.WriteLine("6. Exit editing\n");
                        
                        int editOption = int.Parse(Console.ReadLine());

                        if (editOption == 1)
                        {
                            // get user input for grocery item information 
                            Console.WriteLine("\nEnter the name of the new item:");
                            string name = Console.ReadLine();
                            Console.WriteLine("Enter the best store to buy the item:");
                            string bestStore = Console.ReadLine();
                            Console.WriteLine("Enter the price of the item:");
                            decimal price = decimal.Parse(Console.ReadLine());
                            Console.WriteLine("Enter the quantity of the item:");
                            int quantity = int.Parse(Console.ReadLine());

                            // add the grocery item to the shopping list
                            shoppingList.AddItem(new Grocery(name, bestStore, price, quantity));
                            Console.WriteLine("Item added.");
                        }
                        else if (editOption == 2)
                        {
                            // get user input for the name of the item to remove
                            Console.WriteLine("\nEnter the name of the item to remove:");
                            string itemToRemove = Console.ReadLine();

                            // remove the item from the shopping list
                            shoppingList.RemoveItem(itemToRemove);
                            Console.WriteLine("\nItem removed if it existed.");
                        }
                        else if (editOption == 3)
                        {
                            // get user input for the name of the item to change quantity and the new quantity
                            Console.WriteLine("\nEnter the name of the item to change quantity:");
                            string nameToChange = Console.ReadLine();
                            Console.WriteLine("Enter the new quantity:");
                            int newQuantity = int.Parse(Console.ReadLine());

                            // change the quantity of the item in the shopping list
                            shoppingList.changeItemQuantity(nameToChange, newQuantity);
                            Console.WriteLine("\nQuantity updated if item exists.");
                        }
                        else if (editOption == 4)
                        {   
                            // get user input for the name of the item to change price and the new price
                            Console.WriteLine("\nEnter the name of the item to change price:");
                            string nameToChange = Console.ReadLine();
                            Console.WriteLine("Enter the new price:");
                            decimal newPrice = decimal.Parse(Console.ReadLine());
                            
                            // change the price of the item in the shopping list
                            shoppingList.changeItemPrice(nameToChange, newPrice);
                            Console.WriteLine("\nPrice updated if item exists.");
                        }
                        else if (editOption == 5)
                        {   
                            // get user input for the name of the item to change best store and the new best store
                            Console.WriteLine("\nEnter the name of the item to change best store:");
                            string nameToChange = Console.ReadLine();
                            Console.WriteLine("Enter the new best store:");
                            string newStore = Console.ReadLine();
                            
                            // change the best store of the item in the shopping list
                            shoppingList.changeItemBestStore(nameToChange, newStore);
                            Console.WriteLine("\nBest store updated if item exists.");
                        }
                        else if (editOption == 6)
                        {
                            Console.WriteLine("\nExiting editing mode.\n");
                            keepEditing = false;
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid input.\n");
                        }
                    }
                }
                // view a shopping list
                else if (option == 6)
                {
                    Console.WriteLine("\nViewing a shopping list...\n");

                    // if there are no shopping lists, prompt the user to create a shopping list first
                    if (allShoppingLists.Count == 0)
                    {
                        Console.WriteLine("You must create a shopping list first.\n");
                    }
                    // if there are shopping lists, prompt the user to select a shopping list to view
                    else
                    {
                        Console.WriteLine("Select the shopping list to view:");

                        // display the shopping lists with their corresponding indices for selection
                        for (int i = 1; i < allShoppingLists.Count + 1; i++)
                        {
                            Console.WriteLine(i + ": " + allShoppingLists[i - 1].Name);
                        }

                        Console.WriteLine("");
                        int listIndex = int.Parse(Console.ReadLine());
                        
                        // print the selected shopping list
                        Console.WriteLine("");
                        Console.WriteLine(allShoppingLists[listIndex - 1].ToString());
                        Console.WriteLine("");
                    }
                }
                // exit the demo
                else if (option == 7)
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

