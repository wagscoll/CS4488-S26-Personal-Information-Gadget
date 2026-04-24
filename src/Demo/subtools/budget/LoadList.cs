namespace Demo_PIG_Tool.BudgetTool;

/* Author: Gabriel Ory
 * This class provides a method to save a ShoppingList object to a text file.
 */
 public class LoadList {

    private string filePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "logs", "shoppingLogs.txt"));
            
    public List<ShoppingList> LoadLists()
    {
        List<ShoppingList> lists = new List<ShoppingList>();

         if (!File.Exists(this.filePath))
            return lists;
        
        string[] lines = File.ReadAllLines(filePath);

        int i = 0;

        // loop through the lines and parse shopping lists
        while (i < lines.Length)
        {

            // look for the start of a shopping list entry
            string line = lines[i].Trim();
            if (line.StartsWith("Shopping List:"))
            {
                string listName = line.Substring("Shopping List:".Length).Trim();
                List<Grocery> items = new List<Grocery>();

                // expect "Items:" next
                i++;
                if (i >= lines.Length || !lines[i].Trim().Equals("Items:", StringComparison.OrdinalIgnoreCase))
                    throw new Exception($"Expected 'Items:' after shopping list name at line {i+1}");

                i++; // move to first item line

                // loop through item lines until we reach the next shopping list or the end of the file
                while (i < lines.Length)
                {
                    // if the line is empty, skip it
                    line = lines[i].Trim();
                    if (string.IsNullOrEmpty(line))
                    {
                        i++;
                        continue;
                    }
                    
                    // if we reach the start of the next shopping list or the total cost line, break out of the loop
                    if (line.StartsWith("Shopping List:")) break; // start of next list
                    if (line.StartsWith("Total Cost:")) 
                    {
                        i++;
                        break; // end of current list
                    }

                    // expect item lines to start with "- " followed by item details
                    if (!line.StartsWith("- ")) { i++; continue; }

                    line = line.Substring(2).Trim(); // remove "- "

                    // parse item details
                    // format: ItemName (quantity @ price each) from Store
                    // example: Apples (4 @ $1.00 each) from Walmart
                    int parenStart = line.IndexOf('(');
                    int parenEnd = line.IndexOf(')');
                    string itemName = line.Substring(0, parenStart).Trim();
                    string qtyPricePart = line.Substring(parenStart + 1, parenEnd - parenStart - 1); // "4 @ $1.00 each"
                    string[] qtyPriceTokens = qtyPricePart.Split('@');
                    int quantity = int.Parse(qtyPriceTokens[0].Trim());
                    decimal price = decimal.Parse(qtyPriceTokens[1].Replace("each", "").Replace("$", "").Trim());
                    int fromIndex = line.IndexOf("from ");
                    int colonIndex = line.IndexOf(':');
                    string store = line.Substring(fromIndex + 5, colonIndex - (fromIndex + 5)).Trim();

                    // create Grocery object and add to items list
                    items.Add(new Grocery(itemName, store, price, quantity));

                    i++;
                }

                lists.Add(new ShoppingList(listName, items));
            }
            else
            {
                i++;
            }
        }

        return lists;
    }
}