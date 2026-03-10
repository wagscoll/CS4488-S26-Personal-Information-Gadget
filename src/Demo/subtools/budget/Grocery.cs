namespace Demo_PIG_Tool.BudgetTool;

/* Author: Gabriel Ory
 * This class represents a grocery item with its name, best store to buy from, price, and quantity.
 * It provides a method to calculate the total cost of the grocery item.
 */

public class Grocery
{
    public string Name { get; } = "";
    public string BestStore { get; set;} = "";
    public decimal Price { get; set;}
    public int Quantity { get; set; }

    // constructor
    public Grocery(string name, string bestStore, decimal price, int quantity)
    {
        this.Name = name;
        this.BestStore = bestStore;
        this.Price = price;
        this.Quantity = quantity;
    }

    // calculates the total cost of the grocery item based on its price and quantity
    public decimal TotalCost()
    {
        return Price * Quantity;
    }
}   

