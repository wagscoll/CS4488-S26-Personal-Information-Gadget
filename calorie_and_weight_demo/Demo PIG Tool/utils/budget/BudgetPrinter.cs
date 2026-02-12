using System;
using Demo_PIG_Tool.Utils.Budget;

/* Author: Gabriel Ory 
 * This class provides a method to print the details of a Budget object in a readable format. 
 */

namespace Demo_PIG_Tool.Utils.Budget
{
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
}
