using System;
using System.Collections.Generic;
using System.IO;
namespace Demo_PIG_Tool.BudgetTool;

/* Author: Gabriel Ory
 * This class provides a method to save a list of Budget objects to a text file. 
 * The budgets are saved in a specific format, with each budget separated by "---BUDGET---" and 
 * containing lines for month, year, income, and each category amount.
 */

public class SaveBudget {
    private string filePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "logs", "budgetLogs.txt"));
    public void SaveBudgets(List<Budget> budgets)
    {
        // use a StreamWriter to write the budget details to the specified file path
        using (StreamWriter sw = new StreamWriter(this.filePath, false))
        {
            // loop through each Budget object in the list and write its details to the file
            foreach (Budget budget in budgets)
            {
                sw.WriteLine("---BUDGET---");
                sw.WriteLine("Month=" + budget.Month);
                sw.WriteLine("Year=" + budget.Year);
                sw.WriteLine("Income=" + budget.MonthlyIncome);
                sw.WriteLine();

                // loop through each Category in the Budget and write its details to the file
                foreach (Category c in budget.Categories)
                {
                    sw.WriteLine(
                        c.Name + "|" +
                        c.CategoryBudget + "|" +
                        c.MoneySpent() + "|"
                    );

                    // loop through each Expense in the Category and write its details to the file
                    foreach (Expense e in c.Expenses)
                    {
                        sw.WriteLine(
                            "- " +
                            e.Amount + "|" +
                            e.Description + "|" +
                            e.Date.ToString("yyyy-MM-dd")
                        );
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}