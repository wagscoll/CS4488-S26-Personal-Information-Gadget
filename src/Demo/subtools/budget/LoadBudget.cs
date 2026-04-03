using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
namespace Demo_PIG_Tool.BudgetTool;

/* Author: Gabriel Ory
 * This class provides a method to load Budget objects from a text file. The file should be formatted 
 * in a specific way, with each budget separated by "---BUDGET---" and containing lines for month, year,
 * income, and each category amounts.
 */

public class LoadBudget
{
    // path to the budget log file
    private string filePath = Path.GetFullPath(
        Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "logs", "budgetLogs.txt")
    );

    public List<Budget> LoadBudgets()
    {
        // initialize an empty list to hold the loaded budgets
        List<Budget> budgets = new List<Budget>();

        // check if the file exists (and if it doesn't exist, return the empty list)
        if (!File.Exists(this.filePath))
            return budgets;

        // read all lines from the file
        string[] lines = File.ReadAllLines(this.filePath);
        int i = 0;

        // loop through the lines and parse the budget details
        while (i < lines.Length)
        {
            string line = lines[i].Trim();

            // look for the start of a budget entry
            if (line.StartsWith("Month=", StringComparison.OrdinalIgnoreCase))
            {
                // parse the month, year, and income from the subsequent lines
                int month = int.Parse(lines[i].Trim().Split('=')[1]);
                int year = int.Parse(lines[i + 1].Trim().Split('=')[1]);
                decimal income = decimal.Parse(lines[i + 2].Trim().Split('=')[1]);

                Budget budget = new Budget(month, year, income);

                int lineIndex = i + 3; // start reading categories after Month/Year/Income
                int categoryIndex = 0;

                // loop through the lines until we reach the next budget entry or the end of the file
                while (lineIndex < lines.Length &&
                       !lines[lineIndex].Trim().StartsWith("Month=", StringComparison.OrdinalIgnoreCase))
                {
                    string currentLine = lines[lineIndex].Trim();

                    if (string.IsNullOrEmpty(currentLine))
                    {
                        lineIndex++;
                        continue;
                    }

                    // check if the line is a category line or an expense line
                    if (!currentLine.StartsWith("-"))
                    {
                        // category line format: Name|Budgeted|Spent|
                        string[] parts = currentLine.Split('|');
                        decimal budgeted = decimal.Parse(parts[1]);
                        decimal spent = decimal.Parse(parts[2]);

                        var category = budget.Categories[categoryIndex];
                        category.CategoryBudget = budgeted;
                        category.Expenses.Clear();

                        categoryIndex++;
                    }
                    // if the line starts with "-", it's an expense line for the current category
                    else
                    {
                        // expense line format: - amount|description|date
                        string expenseData = currentLine.Substring(1).Trim();
                        string[] parts = expenseData.Split('|');

                        decimal amount = decimal.Parse(parts[0]);
                        string description = parts[1];
                        DateTime date = DateTime.Parse(parts[2]);

                        var category = budget.Categories[categoryIndex - 1];
                        string categoryName = category.Name;

                        category.Expenses.Add(new Expense(date, description, amount, categoryName));
                    }

                    lineIndex++;
                }

                budgets.Add(budget);
                i = lineIndex;
            }
            else
            {
                i++;
            }
        }

        return budgets;
    }
}
