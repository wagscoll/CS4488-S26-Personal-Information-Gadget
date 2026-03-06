namespace Demo_PIG_Tool.BudgetTool;

/* Author: Gabriel Ory
 * This class provides a method to save a ShoppingList object to a text file.
 */

 public class SaveList {

    private string filePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "logs", "shoppingLogs.txt"));
            
    public void SaveLists(List<ShoppingList> lists)
    {
        // write the shopping lists to the file, overwriting any existing content
        using (StreamWriter sw = new StreamWriter(filePath, false)){
            
            // loop through each shopping list and write its details to the file
            foreach (var list in lists)
            {
                sw.WriteLine($"Shopping List: {list.Name}");
                sw.WriteLine("Items:");

                // loop through each item in the shopping list and write its details to the file
                foreach (var item in list.Items)
                {
                    decimal total = item.Price * item.Quantity;
                    sw.WriteLine(
                        $"- {item.Name} ({item.Quantity} @ ${item.Price:F2} each) from {item.BestStore}: ${total:F2}"
                    );
                }

                // calculate and write the total cost of the shopping list to the file
                decimal totalCost = list.Items.Sum(i => i.Price * i.Quantity);
                sw.WriteLine($"Total Cost: ${totalCost:F2}");
                sw.WriteLine();
            }
        }
    }
}