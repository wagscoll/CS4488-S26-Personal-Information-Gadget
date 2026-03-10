using System;
namespace Demo_PIG_Tool.BudgetTool;


/* Author: Gabriel Ory 
 * This class provides a method to print the details of a Budget object in a readable format. 
 */


public static class BudgetPrinter
{
    public static void Print(Budget budget)
    {
        Console.WriteLine($"Income: ${budget.MonthlyIncome}");
        Console.WriteLine($"Budget {budget.Month}/{budget.Year}");
        Console.WriteLine();

        foreach (var c in budget.Categories)
        {
            Console.WriteLine($"{c.Name}");
            Console.WriteLine($"  Category Budget: ${c.CategoryBudget}");
            Console.WriteLine($"  Money Spent: ${c.MoneySpent()}");
            Console.WriteLine($"  Category Budget Remaining: ${c.Remaining()}");
            Console.WriteLine();
        }

        Console.WriteLine($"Total Money Spent: ${budget.TotalSpent()}");
        Console.WriteLine($"Remaining Income: ${budget.RemainingIncome()}");
    }
}

