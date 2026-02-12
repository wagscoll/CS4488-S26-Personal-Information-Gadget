using System;
using System.Collections.Generic;
using Demo_PIG_Tool.Utils.Budget;

/* Author: Gabriel Ory 
 * This class represents a monthly budget for a specific month and year.
 * It contains the monthly income and a list of categories, each with its own budget and expenses. 
 * The class provides methods to calculate the total budget, total spent, remaining income, and to add expenses to categories.
 */

namespace Demo_PIG_Tool.Utils.Budget
{
    public class Budget
    {
        public int Month { get; }
        public int Year { get; }
        public decimal MonthlyIncome { get; }
        public List<Category> Categories { get; }

        // constructor initializes the monthly budget with default categories
        public Budget(int month, int year, decimal monthlyIncome)
        {
            this.Month = month;
            this.Year = year;
            this.MonthlyIncome = monthlyIncome;

            this.Categories = new List<Category>
            {
                new Category("Savings"),
                new Category("Insurance"),
                new Category("Rent/Mortgage"),
                new Category("Gas"),
                new Category("Food"),
                new Category("Fun"),
                new Category("Other"),
            };
        }

        // calculates the total budget across all categories
        public decimal TotalBudget()
        {
            decimal totalBudget = 0;
            foreach (var c in Categories)
            {
                totalBudget += c.CategoryBudget;
            }
            return totalBudget;
        }

        // calculates the total money spent across all categories
        public decimal TotalSpent()
        {
            decimal totalSpent = 0;
            foreach (var c in Categories)
            {
                totalSpent += c.MoneySpent();
            }
            return totalSpent;
        }

        // calculates the remaining income after all expenses
        public decimal RemainingIncome()
        {
            return MonthlyIncome - TotalSpent();
        }

        // gets a category by name, returns null if not found
        public Category? GetCategory(string name)
        {
            foreach (var c in Categories)
            {
                if (c.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return c;
                }
            }
            return null;
        }

        // add an expense to a category if the category exists or to the "Other" category if it does not exist
        public void AddExpense(Expense expense)
        {
            var category = GetCategory(expense.CategoryName);
            if (category != null)
            {
                category.Expenses.Add(expense);
            }
            else
            {
                category = this.Categories[6]; // "Other" category is at index 6
                category.Expenses.Add(expense);
            }
        }
    }
}