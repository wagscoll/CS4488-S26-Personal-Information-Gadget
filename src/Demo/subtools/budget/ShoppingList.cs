namespace Demo_PIG_Tool.BudgetTool;

/* Author: Gabriel Ory
 * This class represents a shopping list containing multiple grocery items.
 * It provides a method to calculate the total cost of all items in the shopping list 
 * and methods to add, remove, and edit grocery items in the list.
 */

public class ShoppingList
{
    public string Name { get; } = "";
    public List<Grocery> Items { get; }

    // constructor
    public ShoppingList(string name, List<Grocery> items)
    {
        this.Name = name;
        this.Items = items;
    }

    // calculates the total cost of all grocery items in the shopping list
    public decimal TotalCost()
    {
        decimal total = 0;
        foreach (var item in Items)
        {
            total += item.TotalCost();
        }
        return total;
    }

    // add a grocery item to the shopping list
    public void AddItem(Grocery item)
    {
        Items.Add(item);
    }

    // find and remove a grocery item by name
    public void RemoveItem(string itemName)
    {   
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Name == itemName)
            {
                Items.RemoveAt(i);
                break;
            }
        }
    }

    // change the quantity of a grocery item by name
    public void changeItemQuantity(string itemName, int newQuantity)
    {
        foreach (var item in Items)
        {
            if (item.Name == itemName)
            {
                item.Quantity = newQuantity;
                break;
            }
        }
    }

    // change the price of a grocery item by name
    public void changeItemPrice(string itemName, decimal newPrice)
    {
        foreach (var item in Items)
        {
            if (item.Name == itemName)
            {
                item.Price = newPrice;
                break;
            }
        }
    }

    // change the best store for a grocery item by name
    public void changeItemBestStore(string itemName, string newBestStore)
    {
        foreach (var item in Items)
        {
            if (item.Name == itemName)
            {
                item.BestStore = newBestStore;
                break;
            }
        }
    }

    // override ToString method for display
    public override string ToString()
    {
        string itemList = "";
        foreach (var item in Items)
        {
            itemList += $"- {item.Name} ({item.Quantity} @ {item.Price:C} each) from {item.BestStore}: {item.TotalCost():C}\n";
        }
        return $"Shopping List: {Name}\nItems:\n{itemList}Total Cost: {TotalCost():C}";
    }
}