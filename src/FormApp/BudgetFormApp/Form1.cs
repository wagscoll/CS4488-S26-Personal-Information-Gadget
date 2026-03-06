using System;
using System.Collections.Generic;
using System.Drawing;          
using System.Linq;
using System.Windows.Forms;
using Demo_PIG_Tool.BudgetTool;

/* Author: Gabriel Ory
 * This file contains the code for creating the Windows Form App for the Budget and Shopping List sub-tool.
 */

namespace BudgetFormApp
{
    public partial class Form1 : Form
    {
        // theme colors and fonts to match project and task manager
        private static readonly Color PrimaryBlue = Color.FromArgb(0, 120, 215);
        private static readonly Color SelectedBlue = Color.FromArgb(0, 20, 215);
        private static readonly Color LightGray = Color.Gainsboro;
        private static readonly Font UiFont = new Font("Segoe UI", 10, FontStyle.Regular);
        private static readonly Font UiFontBold = new Font("Segoe UI", 10, FontStyle.Bold);

        // helpers to load/save the budget and shopping list
        private readonly LoadBudget _loadBudget = new();
        private readonly SaveBudget _saveBudget = new();
        private readonly LoadList _loadList = new();
        private readonly SaveList _saveList = new();

        // lists of budgets and shopping lists
        private List<Budget> _allBudgets = new();
        private List<ShoppingList> _allShoppingLists = new();

        // menus and tabs
        private readonly MenuStrip menu = new();
        private readonly TabControl tabs = new();

        // budget tab
        private ComboBox cboBudgets = new();
        private Button btnCreateBudget = new();
        private Button btnAddExpense = new();
        private Button btnViewSummary = new();
        private DataGridView gridSummary = new();
        private TextBox txtSummary = new();

        // shopping lists tab
        private ComboBox cboLists = new();
        private Button btnCreateList = new();
        private Button btnAddItem = new();
        private Button btnRemoveItem = new();
        private Button btnEditItem = new();
        private DataGridView gridItems = new();
        private TextBox txtListPreview = new();


        public Form1()
        {
            Text = "Budget and Shopping List Manager";
            Width = 1100;
            Height = 720;
            StartPosition = FormStartPosition.CenterScreen;
            Font = UiFont;

            BuildMenu();
            BuildTabs();

            // load saved budgets and shopping lists on startup
            LoadData();
        }


        // --- STYLES ---

        // blue flat button
        private static void BlueButton(Button b)
        {
            b.BackColor = PrimaryBlue;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = UiFontBold;
            b.Height = 36;
            b.Width = Math.Max(b.Width, 130);
            b.Margin = new Padding(6, 6, 6, 6);
        }

        // gray flat button
        private static void GrayButton(Button b)
        {
            b.BackColor = Color.Gray;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = UiFontBold;
            b.Height = 36;
            b.Width = Math.Max(b.Width, 130);
            b.Margin = new Padding(6, 6, 6, 6);
        }

        // red flat button
        private static void RedButton(Button b)
        {
            b.BackColor = Color.Red;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = UiFontBold;
            b.Height = 36;
            b.Width = Math.Max(b.Width, 130);
            b.Margin = new Padding(6, 6, 6, 6);
        }


        // grid style
        private static void StyleGrid(DataGridView g)
        {
            g.BackgroundColor = Color.White;
            g.BorderStyle = BorderStyle.None;
            g.GridColor = LightGray;

            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersDefaultCellStyle.BackColor = LightGray;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            g.ColumnHeadersDefaultCellStyle.Font = UiFontBold;

            g.DefaultCellStyle.Font = UiFont;
            g.RowHeadersVisible = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.MultiSelect = false;
        }

        // textbox style
        private static void StyleReadOnlyTextBox(TextBox t)
        {
            t.BackColor = Color.White;
            t.BorderStyle = BorderStyle.FixedSingle;
            t.Font = UiFont;
        }


        // build the top menu with file > reload, save, exit
        private void BuildMenu()
        {
            var file = new ToolStripMenuItem("&File");
            var reload = new ToolStripMenuItem("&Reload (Load from disk)", null, (_, __) => LoadData());
            var save = new ToolStripMenuItem("&Save Now", null, (_, __) => SaveData());
            var exit = new ToolStripMenuItem("E&xit", null, (_, __) => Close());

            file.DropDownItems.Add(reload);
            file.DropDownItems.Add(save);
            file.DropDownItems.Add(exit);

            menu.Items.Add(file);

            // menu style
            menu.Dock = DockStyle.Top;
            MainMenuStrip = menu;

            Controls.Add(menu);
        }


        // build the tab control with two tabs: budgets and shopping lists
        private void BuildTabs()
        {
            // make the tab control fill the form
            tabs.Dock = DockStyle.Fill;

            // make tabs larger
            tabs.SizeMode = TabSizeMode.Fixed;
            tabs.ItemSize = new Size(200, 90);

            // set font for tab titles
            tabs.Font = UiFontBold;

            // add the tab control to the form
            Controls.Add(tabs);

            // create the two tabs
            BuildBudgetsTab();
            BuildShoppingTab();
        }


        // build the budgets tab
        private void BuildBudgetsTab()
        {
            // create the tab page
            var page = new TabPage("Budgets") { BackColor = Color.White };
            tabs.TabPages.Add(page);

            // top bar 
            var top = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(10),
                AutoScroll = true
            };

            // label for budget dropdown
            var lbl = new Label
            {
                Text = "Select Budget:",
                AutoSize = true,
                Padding = new Padding(0, 10, 0, 0),
                Font = UiFontBold
            };

            // budget dropdown
            cboBudgets.Width = 240;
            cboBudgets.Font = UiFont;
            cboBudgets.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBudgets.SelectedIndexChanged += (_, __) => RenderBudgetSummary();

            // create budget button
            btnCreateBudget.Text = "Create Budget";
            btnCreateBudget.Click += (_, __) => CreateBudgetDialog();
            BlueButton(btnCreateBudget);

            // add expense button
            btnAddExpense.Text = "Add Expense";
            btnAddExpense.Click += (_, __) => AddExpenseDialog();
            BlueButton(btnAddExpense);

            // view summary button
            btnViewSummary.Text = "View Summary";
            btnViewSummary.Click += (_, __) => RenderBudgetSummary();
            GrayButton(btnViewSummary);

            // add controls to the top action bar
            top.Controls.Add(lbl);
            top.Controls.Add(cboBudgets);
            top.Controls.Add(btnCreateBudget);
            top.Controls.Add(btnAddExpense);
            top.Controls.Add(btnViewSummary);

            // split container for summary grid (top) and text summary (bottom)
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 350
            };
           
            // summary grid style
            gridSummary.Dock = DockStyle.Fill;
            gridSummary.ReadOnly = true;
            gridSummary.AllowUserToAddRows = false;
            gridSummary.AllowUserToDeleteRows = false;
            gridSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            StyleGrid(gridSummary);

            // summary text style
            txtSummary.Dock = DockStyle.Fill;
            txtSummary.Multiline = true;
            txtSummary.ScrollBars = ScrollBars.Vertical;
            txtSummary.ReadOnly = true;
            StyleReadOnlyTextBox(txtSummary);

            // add the grid and text box to the split panels
            split.Panel1.Controls.Add(gridSummary);
            split.Panel2.Controls.Add(txtSummary);

            // add top and split to the page 
            page.Controls.Add(top);
            page.Controls.Add(split);

        }


        // build the shopping lists tab
        private void BuildShoppingTab()
        {
        
            var page = new TabPage("Shopping Lists") { BackColor = Color.White };
            tabs.TabPages.Add(page);

            // top bar 
            var top = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(10),
                AutoScroll = true
            };

            // label for shopping list dropdown
            var lbl = new Label
            {
                Text = "Select List:",
                AutoSize = true,
                Padding = new Padding(0, 10, 0, 0),
                Font = UiFontBold
            };

            // shopping list dropdown
            cboLists.Width = 240;
            cboLists.Font = UiFont;
            cboLists.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLists.SelectedIndexChanged += (_, __) => RenderListItems();

            // create list button
            btnCreateList.Text = "Create List";
            btnCreateList.Click += (_, __) => CreateList();
            BlueButton(btnCreateList);

            // add item button
            btnAddItem.Text = "Add Item";
            btnAddItem.Click += (_, __) => AddItemDialog();
            BlueButton(btnAddItem);

            // edit item button
            btnEditItem.Text = "Edit Item";
            btnEditItem.Click += (_, __) => EditItemDialog();
            BlueButton(btnEditItem);

            // remove item button
            btnRemoveItem.Text = "Remove Item";
            btnRemoveItem.Click += (_, __) => RemoveItemDialog();
            RedButton(btnRemoveItem);

            // add controls to the top action bar
            top.Controls.Add(lbl);
            top.Controls.Add(cboLists);
            top.Controls.Add(btnCreateList);
            top.Controls.Add(btnAddItem);
            top.Controls.Add(btnEditItem);
            top.Controls.Add(btnRemoveItem);

            // split container for items grid (top) and list preview (bottom)
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 350
            };

            // items grid style
            gridItems.Dock = DockStyle.Fill;
            gridItems.ReadOnly = true;
            gridItems.AllowUserToAddRows = false;
            gridItems.AllowUserToDeleteRows = false;
            gridItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            StyleGrid(gridItems);

            // preview text style
            txtListPreview.Dock = DockStyle.Fill;
            txtListPreview.Multiline = true;
            txtListPreview.ScrollBars = ScrollBars.Vertical;
            txtListPreview.ReadOnly = true;
            StyleReadOnlyTextBox(txtListPreview);

            // add the grid and text box to the split panels
            split.Panel1.Controls.Add(gridItems);
            split.Panel2.Controls.Add(txtListPreview);

            // add top and split to the page 
            page.Controls.Add(top);
            page.Controls.Add(split);
        }


        // load budgets and shopping lists from log files
        private void LoadData()
        {
            // try to load budgets, if it fails show a warning and start with an empty list
            try
            {
                _allBudgets = _loadBudget.LoadBudgets() ?? new List<Budget>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to load budgets.\n\n{ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _allBudgets = new List<Budget>();
            }

            // try to load shopping lists, if it fails show a warning and start with an empty list
            try
            {
                _allShoppingLists = _loadList.LoadLists() ?? new List<ShoppingList>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to load shopping lists.\n\n{ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _allShoppingLists = new List<ShoppingList>();
            }

            // after loading, refresh the dropdowns and render summaries/items
            RefreshBudgetDropdown();
            RefreshListDropdown();
            RenderBudgetSummary();
            RenderListItems();
        }


        // save both budgets and shopping lists to log files
        private void SaveData()
        {
            // try to save budgets, if it fails show a warning and stop 
            try
            {
                _saveBudget.SaveBudgets(_allBudgets);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to save budgets.\n\n{ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // try to save shopping lists, if it fails show a warning and stop
            try
            {
                _saveList.SaveLists(_allShoppingLists);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to save shopping lists.\n\n{ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            MessageBox.Show(this, "Saved successfully.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // save budgets only (called after budget changes)
        private void SaveBudgets()
        {
            try { _saveBudget.SaveBudgets(_allBudgets); }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to save budgets.\n\n{ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Save shopping lists only (called after list changes)
        private void SaveLists()
        {
            try { _saveList.SaveLists(_allShoppingLists); }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to save shopping lists.\n\n{ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

 
        // create a new budget using the dialog input
        private void CreateBudgetDialog()
        {
            using var dlg = new BudgetCreateForm();
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            // create a new budget with the dialog values and add it to the list
            var budget = new Budget(dlg.Month, dlg.Year, dlg.MonthlyIncome);
            budget.Categories[0].CategoryBudget = dlg.Savings;
            budget.Categories[1].CategoryBudget = dlg.Insurance;
            budget.Categories[2].CategoryBudget = dlg.RentMortgage;
            budget.Categories[3].CategoryBudget = dlg.Gas;
            budget.Categories[4].CategoryBudget = dlg.Food;
            budget.Categories[5].CategoryBudget = dlg.Fun;
            budget.Categories[6].CategoryBudget = dlg.Other;

            _allBudgets.Add(budget);

            // save after creating a budget
            SaveBudgets();

            // refresh dropdown and render summary for the new budget
            RefreshBudgetDropdown(selectBudget: budget);
            RenderBudgetSummary();
        }

        // add a new expense to the selected budget
        private void AddExpenseDialog()
        {
            // get the currently selected budget, if none is selected show a warning and return
            var budget = GetSelectedBudget();
            if (budget == null)
            {
                MessageBox.Show(this, "Create/select a budget first.", "No Budget", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dlg = new ExpenseAddForm();
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            // add the new expense to the budget with the dialog values
            budget.AddExpense(new Expense(dlg.Date, dlg.Description, dlg.Amount, dlg.CategoryName));

            // save after adding an expense
            SaveBudgets();

            // render the summary to show the new expense
            RenderBudgetSummary();
        }

        // show budget totals by category in the grid + the text summary
        private void RenderBudgetSummary()
        {
            // get the currently selected budget, if none is selected clear the grid and show a message in the text box
            var budget = GetSelectedBudget();
            if (budget == null)
            {
                gridSummary.DataSource = null;
                txtSummary.Text = "No budget selected.";
                return;
            }

            // select the category totals and put them in an anonymous object list for the grid data source
            var rows = budget.Categories.Select(c => new
            {
                Category = c.Name,
                Budget = c.CategoryBudget,
                Spent = c.MoneySpent(),
                Remaining = c.Remaining()
            }).ToList();

            // set the grid data source to the category totals
            gridSummary.DataSource = rows;

            // build the text summary with the budget info and category totals
            txtSummary.Text =
                $"Budget: {budget.Month}/{budget.Year}\r\n" +
                $"Monthly Income: {budget.MonthlyIncome:C}\r\n\r\n" +
                string.Join(Environment.NewLine, rows.Select(r =>
                    $"{r.Category,-14}  Budget {r.Budget,10:C}  Spent {r.Spent,10:C}  Remaining {r.Remaining,10:C}"
                ));
        }

        // get currently selected budget from the dropdown
        private Budget? GetSelectedBudget() => cboBudgets.SelectedItem as Budget;

        // refresh budget dropdown items
        private void RefreshBudgetDropdown(Budget? selectBudget = null)
        {
            // refresh the budget dropdown with the current list of budgets, showing "Month/Year" as the display member
            cboBudgets.BeginUpdate();
            cboBudgets.DataSource = null;
            cboBudgets.DisplayMember = ""; 
            cboBudgets.DataSource = _allBudgets.ToList();
            cboBudgets.EndUpdate();

            // after refreshing, try to re-select the previously selected budget, or select the last one if there are any
            if (selectBudget != null) cboBudgets.SelectedItem = selectBudget;
            else if (_allBudgets.Count > 0) cboBudgets.SelectedIndex = _allBudgets.Count - 1;
        }

  

        // create a new empty shopping list
        private void CreateList()
        {
            var name = Prompt("Shopping List Name", "Enter the name of the shopping list:");
            if (string.IsNullOrWhiteSpace(name)) return;

            // create a new shopping list with the given name and add it to the list
            var list = new ShoppingList(name.Trim(), new List<Grocery>());
            _allShoppingLists.Add(list);

            // save immediately after creating a list
            SaveLists();

            // refresh dropdown and render the empty list
            RefreshListDropdown(selectList: list);
            RenderListItems();
        }

        // add a grocery item to the selected list
        private void AddItemDialog()
        {
            // get the currently selected shopping list, if none is selected show a warning and return
            var list = GetSelectedList();
            if (list == null)
            {
                MessageBox.Show(this, "Create/select a shopping list first.", "No List", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dlg = new GroceryAddForm();
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            // add a new grocery item to the list with the dialog values
            list.AddItem(new Grocery(dlg.ItemName, dlg.BestStore, dlg.Price, dlg.Quantity));

            // save after adding an item
            SaveLists();

            // render the list items to show the new item
            RenderListItems();
        }

        // remove an item by name
        private void RemoveItemDialog()
        {
            // get the currently selected shopping list, if none is selected show a warning and return
            var list = GetSelectedList();
            if (list == null) return;

            // prompt for the name of the item to remove, if it's empty or whitespace return
            var name = Prompt("Remove Item", "Enter the name of the item to remove:");
            if (string.IsNullOrWhiteSpace(name)) return;

            // remove the item with the given name from the list
            list.RemoveItem(name.Trim());

            // save immediately after removing an item
            SaveLists();

            // re-render the list items to show the updated list
            RenderListItems();
        }

        // edit item fields (quantity/price/store)
        private void EditItemDialog()
        {
            // get the currently selected shopping list, if none is selected show a warning and return
            var list = GetSelectedList();
            if (list == null) return;

            // open the edit dialog, which allows changing the quantity, price, or best store of an item by name
            using var dlg = new GroceryEditForm();
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            // based on the dialog action, call the appropriate method on the shopping list to edit the item
            if (dlg.Action == GroceryEditAction.ChangeQuantity)
                list.changeItemQuantity(dlg.TargetName, dlg.NewQuantity);
            else if (dlg.Action == GroceryEditAction.ChangePrice)
                list.changeItemPrice(dlg.TargetName, dlg.NewPrice);
            else if (dlg.Action == GroceryEditAction.ChangeBestStore)
                list.changeItemBestStore(dlg.TargetName, dlg.NewBestStore);

            // save after editing
            SaveLists();

            // re-render the list items to show the updated item
            RenderListItems();
        }

        // show list items in the grid and preview in the textbox
        private void RenderListItems()
        {
            // get the currently selected shopping list, if none is selected clear the grid and show a message in the text box
            var list = GetSelectedList();
            if (list == null)
            {
                gridItems.DataSource = null;
                txtListPreview.Text = "No shopping list selected.";
                return;
            }

            // select the item information and put them in an anonymous object list for the grid data source
            gridItems.DataSource = list.Items.Select(i => new
            {
                i.Name,
                i.BestStore,
                i.Price,
                i.Quantity,
                Total = i.TotalCost()
            }).ToList();

            // build the text preview with the list name, total cost, and item details
            txtListPreview.Text = list.ToString();
        }

        // get currently selected list from the dropdown
        private ShoppingList? GetSelectedList() => cboLists.SelectedItem as ShoppingList;

        // refresh shopping list dropdown items
        private void RefreshListDropdown(ShoppingList? selectList = null)
        {
            // refresh the shopping list dropdown with the current list of shopping lists, showing the list name as the display member
            cboLists.BeginUpdate();
            cboLists.DataSource = null;
            cboLists.DisplayMember = "Name";
            cboLists.DataSource = _allShoppingLists.ToList();
            cboLists.EndUpdate();

            // after refreshing, try to re-select the previously selected list, or select the first one if there are any
            if (selectList != null) cboLists.SelectedItem = selectList;
            else if (_allShoppingLists.Count > 0) cboLists.SelectedIndex = 0;
        }

      
        // input box used for “Create List” and “Remove Item”
        private static string? Prompt(string title, string message)
        {
            // create a simple form with a label, textbox, and OK/Cancel buttons for input
            using var form = new Form
            {
                Text = title,
                Width = 520,
                Height = 170,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                Font = UiFont
            };

            // label for the message, and textbox for input
            var lbl = new Label { Left = 12, Top = 12, Width = 480, Text = message, Font = UiFontBold };
            var txt = new TextBox { Left = 12, Top = 40, Width = 480, Font = UiFont };

            // panel for the ok and cancel buttons
            var panel = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 54, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(12) };
            var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, Width = 110, Height = 36 };
            var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 110, Height = 36 };

            // match blue/gray flat styles for the buttons
            ok.BackColor = Color.FromArgb(0, 120, 215);
            ok.ForeColor = Color.White;
            ok.FlatStyle = FlatStyle.Flat;
            ok.FlatAppearance.BorderSize = 0;
            cancel.BackColor = Color.Gray;
            cancel.ForeColor = Color.White;
            cancel.FlatStyle = FlatStyle.Flat;
            cancel.FlatAppearance.BorderSize = 0;

            // add buttons to the panel and panel to the form
            panel.Controls.Add(ok);
            panel.Controls.Add(cancel);

            // add controls to the form and set the accept/cancel buttons
            form.Controls.Add(panel);
            form.Controls.Add(lbl);
            form.Controls.Add(txt);
            form.AcceptButton = ok;
            form.CancelButton = cancel;

            return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
        }
    }


    // dialog for creating a new budget, with inputs for month/year, monthly income, and category budgets. 
    internal sealed class BudgetCreateForm : Form
    {
        public int Month => (int)numMonth.Value;
        public int Year => (int)numYear.Value;
        public decimal MonthlyIncome => numIncome.Value;

        public decimal Savings => numSavings.Value;
        public decimal Insurance => numInsurance.Value;
        public decimal RentMortgage => numRent.Value;
        public decimal Gas => numGas.Value;
        public decimal Food => numFood.Value;
        public decimal Fun => numFun.Value;
        public decimal Other => numOther.Value;

        private NumericUpDown numMonth = new();
        private NumericUpDown numYear = new();
        private NumericUpDown numIncome = new();

        private NumericUpDown numSavings = new();
        private NumericUpDown numInsurance = new();
        private NumericUpDown numRent = new();
        private NumericUpDown numGas = new();
        private NumericUpDown numFood = new();
        private NumericUpDown numFun = new();
        private NumericUpDown numOther = new();

        // the constructor builds the form layout and initializes the controls
        public BudgetCreateForm()
        {
            Text = "Create Budget";
            Width = 520;
            Height = 520;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 10);

            // use a table layout panel for the form inputs, with 2 columns (label + input)
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));

            // initialize the numeric up-down controls with appropriate ranges and defaults
            numMonth.Minimum = 1; numMonth.Maximum = 12; numMonth.Value = DateTime.Now.Month;
            numYear.Minimum = 2000; numYear.Maximum = 3000; numYear.Value = DateTime.Now.Year;

            // for the money inputs, set them to have 2 decimal places, a reasonable max, and a $50 increment
            SetupMoney(numIncome);
            SetupMoney(numSavings);
            SetupMoney(numInsurance);
            SetupMoney(numRent);
            SetupMoney(numGas);
            SetupMoney(numFood);
            SetupMoney(numFun);
            SetupMoney(numOther);

            // add the controls to the table with labels
            AddRow(table, "Month (1-12)", numMonth);
            AddRow(table, "Year (YYYY)", numYear);
            AddRow(table, "Monthly Income", numIncome);

            // add a header for the category budgets section
            AddHeader(table, "Category Budgets");

            // add rows for each category budget input
            AddRow(table, "Savings", numSavings);
            AddRow(table, "Insurance", numInsurance);
            AddRow(table, "Rent/Mortgage", numRent);
            AddRow(table, "Gas", numGas);
            AddRow(table, "Food", numFood);
            AddRow(table, "Fun", numFun);
            AddRow(table, "Other", numOther);

            // panel for the Create and Cancel buttons
            var panel = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 54, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(12) };
            var ok = new Button { Text = "Create", DialogResult = DialogResult.OK, Width = 110, Height = 36 };
            var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 110, Height = 36 };

            // match blue/gray flat styles for the buttons
            ok.BackColor = Color.FromArgb(0, 120, 215);
            ok.ForeColor = Color.White;
            ok.FlatStyle = FlatStyle.Flat;
            ok.FlatAppearance.BorderSize = 0;
            cancel.BackColor = Color.Gray;
            cancel.ForeColor = Color.White;
            cancel.FlatStyle = FlatStyle.Flat;
            cancel.FlatAppearance.BorderSize = 0;

            // add buttons to the panel and panel to the form
            panel.Controls.Add(ok);
            panel.Controls.Add(cancel);
            Controls.Add(table);
            Controls.Add(panel);

            // set the accept button to Create and the cancel button to Cancel
            AcceptButton = ok;
            CancelButton = cancel;
        }

        // helper method to set up the money input controls with consistent settings
        private static void SetupMoney(NumericUpDown n)
        {
            n.DecimalPlaces = 2;
            n.Maximum = 1_000_000;
            n.Minimum = 0;
            n.ThousandsSeparator = true;
            n.Increment = 50;
            n.Width = 160;
        }

        // helper method to add a header row that spans both columns
        private static void AddHeader(TableLayoutPanel table, string text)
        {
            int row = table.RowCount;
            table.RowCount++;
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var lbl = new Label { Text = text, AutoSize = true, Padding = new Padding(0, 12, 0, 0), Font = new Font("Segoe UI", 11, FontStyle.Bold) };
            table.Controls.Add(lbl, 0, row);
            table.SetColumnSpan(lbl, 2);
        }

        // helper method to add a label and control in a new row
        private static void AddRow(TableLayoutPanel table, string label, Control control)
        {
            int row = table.RowCount;
            table.RowCount++;
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            table.Controls.Add(new Label { Text = label, AutoSize = true, Padding = new Padding(0, 6, 0, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, row);
            table.Controls.Add(control, 1, row);
        }
    }

    // dialog for adding a new expense to a budget, with inputs for date, description, amount, and category
    internal sealed class ExpenseAddForm : Form
    {
        public DateTime Date => dtp.Value.Date;
        public string Description => txtDesc.Text.Trim();
        public decimal Amount => numAmount.Value;
        public string CategoryName => txtCategory.Text.Trim();

        private DateTimePicker dtp = new();
        private TextBox txtDesc = new();
        private NumericUpDown numAmount = new();
        private TextBox txtCategory = new();

        // the constructor builds the form layout and initializes the controls
        public ExpenseAddForm()
        {
            Text = "Add Expense";
            Width = 520;
            Height = 260;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 10);

            // use a table layout panel for the form inputs, with 2 columns (label + input)
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                ColumnCount = 2,
                RowCount = 5
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

            // initialize the controls with appropriate settings
            dtp.Format = DateTimePickerFormat.Short;
            numAmount.DecimalPlaces = 2;
            numAmount.Maximum = 1_000_000;
            numAmount.Minimum = 0;
            numAmount.ThousandsSeparator = true;

            txtDesc.Width = 300;
            txtCategory.Width = 300;

            // add the controls to the table with labels
            table.Controls.Add(new Label { Text = "Date", AutoSize = true, Padding = new Padding(0, 6, 0, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 0);
            table.Controls.Add(dtp, 1, 0);
            table.Controls.Add(new Label { Text = "Description", AutoSize = true, Padding = new Padding(0, 6, 0, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 1);
            table.Controls.Add(txtDesc, 1, 1);
            table.Controls.Add(new Label { Text = "Amount", AutoSize = true, Padding = new Padding(0, 6, 0, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 2);
            table.Controls.Add(numAmount, 1, 2);
            table.Controls.Add(new Label { Text = "Category", AutoSize = true, Padding = new Padding(0, 6, 0, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 3);
            table.Controls.Add(txtCategory, 1, 3);

            // add a hint label for the category input
            var hint = new Label
            {
                Text = "Categories: Savings, Insurance, Rent/Mortgage, Gas, Food, Fun, Other",
                AutoSize = true
            };
            table.Controls.Add(hint, 0, 4);
            table.SetColumnSpan(hint, 2);

            // panel for the Add and Cancel buttons
            var panel = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 54, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(12) };
            var ok = new Button { Text = "Add", DialogResult = DialogResult.OK, Width = 110, Height = 36 };
            var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 110, Height = 36 };

            // match blue/gray flat styles for the buttons
            ok.BackColor = Color.FromArgb(0, 120, 215);
            ok.ForeColor = Color.White;
            ok.FlatStyle = FlatStyle.Flat;
            ok.FlatAppearance.BorderSize = 0;
            cancel.BackColor = Color.Gray;
            cancel.ForeColor = Color.White;
            cancel.FlatStyle = FlatStyle.Flat;
            cancel.FlatAppearance.BorderSize = 0;

            // add buttons to the panel and panel to the form
            panel.Controls.Add(ok);
            panel.Controls.Add(cancel);
            Controls.Add(table);
            Controls.Add(panel);

            // set the accept button to Add and the cancel button to Cancel
            AcceptButton = ok;
            CancelButton = cancel;
        }
    }

    // dialog for adding a new grocery item to a shopping list, with inputs for name, best store, price, and quantity
    internal sealed class GroceryAddForm : Form
    {
        // values the form reads after OK
        public string ItemName => txtName.Text.Trim();
        public string BestStore => txtStore.Text.Trim();
        public decimal Price => numPrice.Value;
        public int Quantity => (int)numQty.Value;

        // inputs
        private readonly TextBox txtName = new();
        private readonly TextBox txtStore = new();
        private readonly NumericUpDown numPrice = new();
        private readonly NumericUpDown numQty = new();

        // the constructor builds the form layout and initializes the controls
        public GroceryAddForm()
        {
            Text = "Add Grocery Item";
            Width = 520;
            Height = 260;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 10);

            // use a table layout panel for the form inputs, with 2 columns (label + input)
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                ColumnCount = 2,
                RowCount = 4
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

            txtName.Width = 300;
            txtStore.Width = 300;

            // set up the price and quantity inputs with appropriate ranges and formatting
            numPrice.DecimalPlaces = 2;
            numPrice.Maximum = 1_000_000;
            numPrice.Minimum = 0;
            numPrice.ThousandsSeparator = true;
            numQty.Minimum = 1;
            numQty.Maximum = 10_000;

            // add the controls to the table with labels
            table.Controls.Add(new Label { Text = "Name", AutoSize = true, Padding = new Padding(0, 6, 0, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 0);
            table.Controls.Add(txtName, 1, 0);
            table.Controls.Add(new Label { Text = "Best Store", AutoSize = true, Padding = new Padding(0, 6, 0, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 1);
            table.Controls.Add(txtStore, 1, 1);
            table.Controls.Add(new Label { Text = "Price", AutoSize = true, Padding = new Padding(0, 6, 0, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 2);
            table.Controls.Add(numPrice, 1, 2);
            table.Controls.Add(new Label { Text = "Quantity", AutoSize = true, Padding = new Padding(0, 6, 0, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 3);
            table.Controls.Add(numQty, 1, 3);

            // panel for the Add and Cancel buttons
            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 54,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(12)
            };

            // create the Add and Cancel buttons
            var ok = new Button { Text = "Add", DialogResult = DialogResult.OK, Width = 110, Height = 36 };
            var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 110, Height = 36 };

            // match blue/gray flat styles for the buttons
            ok.BackColor = Color.FromArgb(0, 120, 215);
            ok.ForeColor = Color.White;
            ok.FlatStyle = FlatStyle.Flat;
            ok.FlatAppearance.BorderSize = 0;
            cancel.BackColor = Color.Gray;
            cancel.ForeColor = Color.White;
            cancel.FlatStyle = FlatStyle.Flat;
            cancel.FlatAppearance.BorderSize = 0;

            // add buttons to the panel and panel to the form
            panel.Controls.Add(ok);
            panel.Controls.Add(cancel);
            Controls.Add(table);
            Controls.Add(panel);

            // set the accept button to Add and the cancel button to Cancel
            AcceptButton = ok;
            CancelButton = cancel;
        }
    }

    // dialog for editing an existing grocery item in a shopping list, allowing changing the quantity, price, or best store by item name
    internal enum GroceryEditAction
    {
        ChangeQuantity,
        ChangePrice,
        ChangeBestStore
    }

    internal sealed class GroceryEditForm : Form
    {
        // values the main form reads after OK
        public GroceryEditAction Action => (GroceryEditAction)cboAction.SelectedItem!;
        public string TargetName => txtName.Text.Trim();
        public int NewQuantity => (int)numQty.Value;
        public decimal NewPrice => numPrice.Value;
        public string NewBestStore => txtStore.Text.Trim();

        // inputs
        private readonly ComboBox cboAction = new();
        private readonly TextBox txtName = new();
        private readonly NumericUpDown numQty = new();
        private readonly NumericUpDown numPrice = new();
        private readonly TextBox txtStore = new();

        // the constructor builds the form layout and initializes the controls
        public GroceryEditForm()
        {
            Text = "Edit Grocery Item";
            Width = 560;
            Height = 320;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 10);

            // dropdown for selecting the edit action (quantity, price, or best store)
            cboAction.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAction.Items.Add(GroceryEditAction.ChangeQuantity);
            cboAction.Items.Add(GroceryEditAction.ChangePrice);
            cboAction.Items.Add(GroceryEditAction.ChangeBestStore);
            cboAction.SelectedIndex = 0;
            cboAction.SelectedIndexChanged += (_, __) => UpdateEnabledFields();

            // set up the quantity and price inputs with appropriate ranges and formatting
            numQty.Minimum = 1;
            numQty.Maximum = 10_000;
            numPrice.DecimalPlaces = 2;
            numPrice.Maximum = 1_000_000;
            numPrice.Minimum = 0;
            numPrice.ThousandsSeparator = true;

            txtStore.Width = 280;

            // use a table layout panel for the form inputs, with 2 columns (label + input)
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                ColumnCount = 2,
                RowCount = 4
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

            // add the controls to the table with labels
            table.Controls.Add(new Label { Text = "Action", AutoSize = true, Padding = new Padding(0, 6, 0, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 0);
            table.Controls.Add(cboAction, 1, 0);
            table.Controls.Add(new Label { Text = "Item Name", AutoSize = true, Padding = new Padding(0, 6, 0, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 1);
            table.Controls.Add(txtName, 1, 1);
            table.Controls.Add(new Label { Text = "New Quantity", AutoSize = true, Padding = new Padding(0, 6, 0, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 2);
            table.Controls.Add(numQty, 1, 2);
            table.Controls.Add(new Label { Text = "New Price / Store", AutoSize = true, Padding = new Padding(0, 6, 0, 0), Font = new Font("Segoe UI", 10, FontStyle.Bold) }, 0, 3);

            // price OR store (depending on action)
            var panelRight = new FlowLayoutPanel { FlowDirection = FlowDirection.TopDown, AutoSize = true };
            panelRight.Controls.Add(numPrice);
            panelRight.Controls.Add(txtStore);

            table.Controls.Add(panelRight, 1, 3);

            // panel for the Apply and Cancel buttons
            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 54,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(12)
            };

            // create the Apply and Cancel buttons
            var ok = new Button { Text = "Apply", DialogResult = DialogResult.OK, Width = 110, Height = 36 };
            var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Width = 110, Height = 36 };

            // match blue/gray flat styles for the buttons
            ok.BackColor = Color.FromArgb(0, 120, 215);
            ok.ForeColor = Color.White;
            ok.FlatStyle = FlatStyle.Flat;
            ok.FlatAppearance.BorderSize = 0;
            cancel.BackColor = Color.Gray;
            cancel.ForeColor = Color.White;
            cancel.FlatStyle = FlatStyle.Flat;
            cancel.FlatAppearance.BorderSize = 0;

            // add buttons to the panel and panel to the form
            panel.Controls.Add(ok);
            panel.Controls.Add(cancel);
            Controls.Add(table);
            Controls.Add(panel);

            // set the accept button to Apply and the cancel button to Cancel
            AcceptButton = ok;
            CancelButton = cancel;

            // set which input fields are editable based on action
            UpdateEnabledFields();
        }

        // allow only the relevant field for the chosen edit action
        private void UpdateEnabledFields()
        {
            var action = (GroceryEditAction)cboAction.SelectedItem!;
            numQty.Enabled = action == GroceryEditAction.ChangeQuantity;
            numPrice.Enabled = action == GroceryEditAction.ChangePrice;
            txtStore.Enabled = action == GroceryEditAction.ChangeBestStore;
        }
    }
}