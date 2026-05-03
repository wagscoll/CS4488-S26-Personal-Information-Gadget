using System;
namespace Demo_PIG_Tool.BudgetTool;


/* Author: Gabriel Ory
 * This class represents an expense in a budget category.
 * Each expense has a date, a description, an amount, and the name of the category it belongs to.
 */


public class Expense
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = "";
    public decimal Amount { get; set; }
    public string CategoryName { get; set; } = "";

    public Expense(int id, DateTime date, string description, decimal amount, string categoryName)
    {
        this.Id = id;
        this.Date = date;
        this.Description = description;
        this.Amount = amount;
        this.CategoryName = categoryName;
    }
}

