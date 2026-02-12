using System;
using System.Collections.Generic;
namespace Demo_PIG_Tool.BudgetTool;


/* Author: Gabriel Ory
 * This class represents a category in a monthly budget.
 * Each category has a name, a budget, and a list of expenses.
 * The class provides methods to calculate the total money spent and the remaining budget for the category.
 */


    public class Category
    {
        public string Name { get; }
        public decimal CategoryBudget { get; set; }
        public List<Expense> Expenses { get; }

        // constructor initializes the category with a name, a budget, and an empty list of expenses
        public Category(string name)
        {   
            this.Name = name;
            this.CategoryBudget = 0;
            this.Expenses = new List<Expense>();
        }

        // calculates the total money spent in this category
        public decimal MoneySpent()
        {
            decimal moneySpent = 0;
            foreach (var e in Expenses)
            {
                moneySpent += e.Amount;
            }
            return moneySpent;
        }

        // calculates the remaining budget in this category
        public decimal Remaining()
        {
            return CategoryBudget - MoneySpent();
        }
    }

