using System;
namespace Demo_PIG_Tool.BudgetTool;


/* Author: Gabriel Ory
 * This class represents an expense in a budget category.
 * Each expense has a date, a description, an amount, and the name of the category it belongs to.
 */


public class Expense
{
    public DateTime Date { get; }
    public string Description { get; } = "";
    public decimal Amount { get; }
    public string CategoryName { get; } = "";

    public Expense(DateTime date, string description, decimal amount, string categoryName)
    {
        this.Date = date;
        this.Description = description;
        this.Amount = amount;
        this.CategoryName = categoryName;
    }
}

