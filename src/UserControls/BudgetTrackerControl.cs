using Demo_PIG_Tool.BudgetTool;
using Demo_PIG_Tool.Manager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

/* Author: Gabriel Ory
 * This file contains the code for creating the UserControl for the Budget sub-tool.
 */

namespace BudgetTracker
{
    
    public partial class BudgetControl : UserControl
    {
        // helpers to load/save the budget
        private readonly LoadBudget _loadBudget = new();
        private readonly SaveBudget _saveBudget = new();

        // list of all budgets
        private List<Budget> _allBudgets = new();

        // UI components
        private ComboBox cboBudgets = new();
        private Button btnCreateBudget = new();
        private Button btnAddExpense = new();
        private Button btnRemoveExpense = new();
        private Button btnDeleteBudget = new();
        private Panel mainPanel = new Panel();
        private Panel editPanel = new Panel();
        private Label titleLabel = new Label();
        private ListView listViewSummary = new();
        private TextBox txtSummary = new();

        // constructor to set up the UI and load data
        public BudgetControl()
        {
            Dock = DockStyle.Fill;
            BuildUI();
            LoadData();
            SubToolManager.UpdateDocx();
        }

        // builds the user interface with a header, top bar for budget selection and actions, and a split view for the list of expenses and budget summary
        private void BuildUI()
        {

            SubToolManager.UpdateDocx();

            // make header panel
            mainPanel.BackColor = Color.FromArgb(240, 240, 240);
            mainPanel.Dock = DockStyle.Top;
            mainPanel.AutoSize = true;
            mainPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            mainPanel.Padding = new Padding(20, 20, 20, 10); 

            // add title label to the header
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            titleLabel.ForeColor = Color.FromArgb(0, 120, 215);
            titleLabel.Location = new Point(20, 20);
            titleLabel.Text = "Budget Tracking Tool";
            mainPanel.Controls.Add(titleLabel);

            // make top buttons/dropdown panel
            var top = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(10),
                AutoScroll = true
            };

            // label for the dropdown
            var lbl = new Label
            {
                Text = "Select Budget:",
                AutoSize = true,
                Padding = new Padding(0, 10, 0, 0),
            };

            // customize dropdown
            cboBudgets.Width = 240;
            cboBudgets.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBudgets.SelectedIndexChanged += (_, __) => RenderBudgetSummary();

            // button to create a new budget
            btnCreateBudget.Text = "Create Budget";
            btnCreateBudget.Click += (_, __) => ShowCreateBudget();
            BlueButton(btnCreateBudget);

            // button to add a new expense to the selected budget
            btnAddExpense.Text = "Add Expense";
            btnAddExpense.Click += (_, __) => AddExpenseDialog();
            BlueButton(btnAddExpense);

            // button to remove the selected expense from the budget
            btnRemoveExpense.Text = "Remove Expense";
            btnRemoveExpense.Click += (_, __) => RemoveExpense();
            RedButton(btnRemoveExpense);

            // button to delete the selected budget
            btnDeleteBudget.Text = "Delete Budget";
            btnDeleteBudget.Click += (_, __) => DeleteBudget();
            RedButton(btnDeleteBudget);

            // add the label, dropdown, and buttons to the top panel
            top.Controls.Add(lbl);
            top.Controls.Add(cboBudgets);
            top.Controls.Add(btnCreateBudget);
            top.Controls.Add(btnAddExpense);
            top.Controls.Add(btnRemoveExpense);
            top.Controls.Add(btnDeleteBudget);

            // make a split container for ListView and TextBox
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 350
            };

            // panel for editing/adding expenses, which will be shown when the user clicks add/edit and hidden otherwise
            editPanel.Dock = DockStyle.Fill;
            editPanel.Visible = false;
            Controls.Add(editPanel);
            editPanel.BringToFront();

            // customize ListView style
            listViewSummary = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
            };

            // show expenses with columns for date, category, amount, and description
            listViewSummary.Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader { Text = "Date", Width = 200 },
                new ColumnHeader { Text = "Description", Width = 400 },
                new ColumnHeader { Text = "Amount", Width = 200 },
                new ColumnHeader { Text = "Category", Width = 200 },

            });

            // handle double-click on an expense to open the edit form for that expense
            listViewSummary.DoubleClick += ListViewSummary_DoubleClick;

            // customize TextBox style for the summary
            txtSummary.Dock = DockStyle.Fill;
            txtSummary.Multiline = true;
            txtSummary.ScrollBars = ScrollBars.Vertical;
            txtSummary.ReadOnly = true;
            txtSummary.BackColor = Color.White;
            txtSummary.BorderStyle = BorderStyle.FixedSingle;

            // add the ListView to the top panel of the split container and the TextBox to the bottom panel
            split.Panel1.Controls.Add(listViewSummary);
            split.Panel2.Controls.Add(txtSummary);

            // add all panels to the main control
            Controls.Add(split);     
            Controls.Add(top);   
            Controls.Add(mainPanel);  
        }


        // styling for the blue buttons
        private static void BlueButton(Button b)
        {
            b.BackColor = Color.FromArgb(0, 120, 215);
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Size = new Size(130, 35);
            b.Font = new Font("Segoe UI", 10F);
        }

        // styling for the red buttons
        private static void RedButton(Button b)
        {
            b.BackColor = Color.Red;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Size = new Size(130, 35);
            b.Font = new Font("Segoe UI", 10F);
        }

        // styling for the gray buttons
        private static void GrayButton(Button b)
        {
            b.BackColor = Color.Gray;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Size = new Size(130, 35);
            b.Font = new Font("Segoe UI", 10F);
        }

        // when an expense is double-clicked in the ListView, open the edit form for that expense
        private void ListViewSummary_DoubleClick(object? sender, EventArgs e)
        {
            // ensure an item is selected
            if (listViewSummary.SelectedItems.Count == 0) return;

            // get the expense object from the selected ListViewItem's Tag property
            var expense = listViewSummary.SelectedItems[0].Tag as Expense;
            if (expense == null) return;

            // get the currently selected budget to pass to the edit form
            var budget = GetSelectedBudget();
            if (budget == null) return;

            // open the edit form for the selected expense
            ShowEditExpenseForm(budget, expense);
        }

        // removes the selected expense from the budget after confirming with the user
        private void RemoveExpense()
        {
            // ensure a budget is selected
            var budget = GetSelectedBudget();
            if (budget == null) return;

            // ensure an expense is selected
            if (listViewSummary.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select an expense.");
                return;
            }

            // get the expense object from the selected ListViewItem's Tag property
            var item = listViewSummary.SelectedItems[0];
            var expense = item.Tag as Expense;
            if (expense == null) return;

            // find the category that contains this expense, making sure the expense actually belongs to the currently selected budget
            var category = budget.Categories
            .FirstOrDefault(c => c.Expenses.Contains(expense));
            if (category == null) return;

            // confirm with the user before deleting
            var result = MessageBox.Show(
                "Delete this expense?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;
            category.Expenses.Remove(expense);

            // save changes, refresh the ListView, and update the summary
            SaveBudgets();
            RenderBudgetSummary();
        }

        // deletes the currently selected budget after confirming with the user, then refreshes the dropdown and clears the summary view
        private void DeleteBudget()
        {
            // ensure a budget is selected
            var budget = GetSelectedBudget();
            if (budget == null)
            {
                MessageBox.Show("Select a budget first.");
                return;
            }

            // confirm with the user before deleting the budget
            var result = MessageBox.Show(
                $"Delete budget {budget.Month}/{budget.Year}?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;
            _allBudgets.Remove(budget);

            // save changes, refresh the dropdown, clear the ListView and summary textbox, and re-render the summary
            SaveBudgets();
            RefreshBudgetDropdown();
            listViewSummary.Items.Clear();
            txtSummary.Text = "Budget deleted.";
            RenderBudgetSummary();
        }

        // populate the ListView with all expenses for the selected budget and show a text summary of category totals
        private void RenderBudgetSummary()
        {
            SubToolManager.UpdateDocx();
            var budget = GetSelectedBudget();

            // if no budget is selected, clear the ListView and show a message in the summary textbox
            if (budget == null)
            {
                listViewSummary.Items.Clear();
                txtSummary.Text = "No budget selected.";
                return;
            }
            listViewSummary.Items.Clear();

            // flatten all expenses with category
            var allExpenses = budget.Categories
                .SelectMany(c => c.Expenses, (c, e) => new { Category = c.Name, Expense = e })
                .OrderBy(x => x.Expense.Date) // sort by date
                .ToList();

            // add each expense as a row in the ListView
            foreach(var x in allExpenses)
            {
                var item = new ListViewItem(x.Expense.Date.ToShortDateString());
                item.SubItems.Add(x.Category);
                item.SubItems.Add(x.Expense.Amount.ToString("C"));
                item.SubItems.Add(x.Expense.Description);
                item.Tag = x.Expense;
                listViewSummary.Items.Add(item);
            }

            // text summary by category totals 
            txtSummary.Text =
                $"Budget: {budget.Month}/{budget.Year}\r\n" +
                $"Monthly Income: {budget.MonthlyIncome:C}\r\n\r\n" +
                string.Join(Environment.NewLine, budget.Categories.Select(c =>
                    $"{c.Name,-14}  Budget {c.CategoryBudget,10:C}  Spent {c.MoneySpent(),10:C}  Remaining {c.Remaining(),10:$#,0.00;-$#,0.00}"
                ));
        }


        // load budgets from storage and populate the dropdown
        private void LoadData()
        {
            SubToolManager.UpdateDocx();
            _allBudgets = _loadBudget.LoadBudgets();

            // clear existing items 
            cboBudgets.Items.Clear();

            // add each budget to the dropdown
            foreach (var budget in _allBudgets)
                cboBudgets.Items.Add(budget);

            // if there are budgets, select the first one by default
            if (cboBudgets.Items.Count > 0)
                cboBudgets.SelectedIndex = 0;
        }


        // refresh the dropdown with the current budgets and optionally select a specific budget
        private void RefreshBudgetDropdown(Budget? selectBudget = null)
        {
            SubToolManager.UpdateDocx();

            // clear existing items and re-bind the dropdown to the updated list of budgets
            cboBudgets.BeginUpdate();
            cboBudgets.DataSource = null;
            cboBudgets.DisplayMember = "";
            cboBudgets.DataSource = _allBudgets.ToList();
            cboBudgets.EndUpdate();

            // if a specific budget is provided, select it; otherwise, select the last budget in the list
            if (selectBudget != null) cboBudgets.SelectedItem = selectBudget;
            else if (_allBudgets.Count > 0) cboBudgets.SelectedIndex = _allBudgets.Count - 1;
        }


        // get the currently selected budget from the dropdown
        private Budget? GetSelectedBudget() => cboBudgets.SelectedItem as Budget;


        // save budgets (called after budget changes)
        private void SaveBudgets()
        {
            try { 
                _saveBudget.SaveBudgets(_allBudgets);
                SubToolManager.UpdateDocx();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to save budgets.\n\n{ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        // show the form to create a new budget, which includes fields for month, year, income, and category budgets.
        // when the user clicks create, the new budget is added to the list and saved.
        private void ShowCreateBudget()
        {
            // clear the edit panel and show the budget creation form
            editPanel.Controls.Clear();
            editPanel.Visible = true;
            var panel = new BudgetCreatePanel();
            editPanel.Controls.Add(panel);

            // handle the create button click event to create a new budget based on the form inputs, add it to the list, save, and refresh the UI
            panel.BtnCreate.Click += (_, __) =>
            {
                var budget = new Budget(panel.Month, panel.Year, panel.MonthlyIncome);

                budget.Categories[0].CategoryBudget = panel.Savings;
                budget.Categories[1].CategoryBudget = panel.Insurance;
                budget.Categories[2].CategoryBudget = panel.RentMortgage;
                budget.Categories[3].CategoryBudget = panel.Gas;
                budget.Categories[4].CategoryBudget = panel.Food;
                budget.Categories[5].CategoryBudget = panel.Fun;
                budget.Categories[6].CategoryBudget = panel.Other;

                _allBudgets.Add(budget);

                SaveBudgets();
                HideEditPanel();
                RefreshBudgetDropdown();
                RenderBudgetSummary();
            };

            // handle the cancel button click event to hide the edit panel and re-render the summary without making changes
            panel.BtnCancel.Click += (_, __) =>
            {
                HideEditPanel();
                RenderBudgetSummary();
            };
        }


        // add a new expense to the selected budget
        private void AddExpenseDialog()
        {
            // ensure a budget is selected
            var budget = GetSelectedBudget();
            if (budget == null)
            {
                MessageBox.Show(this, "Create/select a budget first.", "No Budget",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // show the form to add a new expense for the selected budget
            ShowAddExpenseForm(budget);
        }

        // show the form to add a new expense, which includes fields for description, amount, date, and category
        private void ShowAddExpenseForm(Budget budget)
        {
            // clear the edit panel and show the expense creation form
            editPanel.Controls.Clear();
            editPanel.Visible = true;
            editPanel.BringToFront();

            // create title label for the form
            var title = new Label
            {
                Text = "Add Expense",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            int y = 70;

            // create label for the expense description
            var lblDesc = new Label
            {
                Text = "Description:",
                Location = new Point(20, y),
                Width = 120
            };

            // create textbox for the expense description
            var txtDesc = new TextBox
            {
                Location = new Point(150, y),
                Width = 300
            };

            editPanel.Controls.Add(lblDesc);
            editPanel.Controls.Add(txtDesc);
            y += 35;

            // create label for the expense amount
            var lblAmount = new Label
            {
                Text = "Amount:",
                Location = new Point(20, y),
                Width = 120
            };

            // create numeric up-down control for the expense amount
            var numAmount = new NumericUpDown
            {
                Location = new Point(150, y),
                Width = 150,
                Maximum = 100000,
                DecimalPlaces = 2
            };

            editPanel.Controls.Add(lblAmount);
            editPanel.Controls.Add(numAmount);
            y += 35;

            // create label for the expense date
            var lblDate = new Label
            {
                Text = "Date:",
                Location = new Point(20, y),
                Width = 120
            };

            // create date picker for the expense date
            var dtDate = new DateTimePicker
            {
                Location = new Point(150, y),
                Width = 200
            };

            editPanel.Controls.Add(lblDate);
            editPanel.Controls.Add(dtDate);
            y += 35;

            // create label for the expense category
            var lblCategory = new Label
            {
                Text = "Category:",
                Location = new Point(20, y),
                Width = 120
            };

            // create dropdown for the expense category
            var cboCategory = new ComboBox
            {
                Location = new Point(150, y),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // populate the category dropdown with the categories from the budget and auto-select the first category by default
            foreach (var c in budget.Categories)
                cboCategory.Items.Add(c.Name);
            cboCategory.SelectedIndex = 0;

            editPanel.Controls.Add(lblCategory);
            editPanel.Controls.Add(cboCategory);
            y += 50;

            // create the button to save the new expense
            var btnSave = new Button
            {
                Text = "Save",
                Location = new Point(180, y),
                Width = 100
            };
            BlueButton(btnSave);

            // handle the save button click event to create a new expense based on the form inputs
            btnSave.Click += (_, __) =>
            {
                // get the selected category object based on the category name selected in the dropdown
                var category = budget.Categories
                    .FirstOrDefault(c => c.Name == cboCategory.SelectedItem?.ToString());

                if (category == null) return;

                // create a new expense object with a unique ID, the form inputs, and the selected category
                var expense = new Expense(
                    GetNextExpenseId(budget),
                    dtDate.Value,
                    txtDesc.Text,
                    (decimal)numAmount.Value,
                    category.Name
                );

                // add the new expense to the selected category in the budget
                category.Expenses.Add(expense);

                // save changes, hide the edit panel, and refresh the ListView and summary
                SaveBudgets();
                HideEditPanel();
                RenderBudgetSummary();
            };
            editPanel.Controls.Add(btnSave);

            // create the button to cancel adding a new expense and return to the summary view
            var btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(320, y),
                Width = 100
            };
            GrayButton(btnCancel);

            // handle the cancel button click event to hide the edit panel and re-render the summary without making changes
            btnCancel.Click += (_, __) =>
            {
                HideEditPanel();
                RenderBudgetSummary();
            };
            editPanel.Controls.Add(btnCancel);
        }

        // helper to get the next unique expense ID for a new expense being added to the budget
        private int GetNextExpenseId(Budget budget)
        {
            // find the maximum existing expense ID across all categories in the budget and return one greater than that for the new expense
            return budget.Categories
                .SelectMany(c => c.Expenses)
                .Select(e => e.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;
        }

        // show the edit form for an existing expense, allowing the user to modify or delete the expense
        private void ShowEditExpenseForm(Budget budget, Expense expense)
        {
            // find the category that contains this expense
            var category = budget.Categories.FirstOrDefault(c => c.Expenses.Any(e => e.Id == expense.Id));
            if (category == null) return;
            var targetExpense = expense;

            // clear the edit panel and show the expense editing form
            editPanel.Controls.Clear();
            editPanel.Visible = true;
            editPanel.BringToFront();

            // create title label for the form, which includes the description of the expense being edited
            var title = new Label
            {
                Text = $"Edit Expense: {targetExpense.Description}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            int y = 70;

            // create label for the expense description
            var lblDesc = new Label
            {
                Text = "Description:",
                Location = new Point(20, y),
                Width = 120
            };

            // create textbox for the expense description
            var txtDesc = new TextBox
            {
                Location = new Point(150, y),
                Width = 300
            };

            editPanel.Controls.Add(lblDesc);
            editPanel.Controls.Add(txtDesc);
            y += 35;

            // create label for the expense amount
            var lblAmount = new Label
            {
                Text = "Amount:",
                Location = new Point(20, y),
                Width = 120
            };

            // create numeric up-down control for the expense amount
            var numAmount = new NumericUpDown
            {
                Location = new Point(150, y),
                Width = 150,
                Maximum = 100000,
                DecimalPlaces = 2
            };

            editPanel.Controls.Add(lblAmount);
            editPanel.Controls.Add(numAmount);
            y += 35;

            // create label for the expense date
            var lblDate = new Label
            {
                Text = "Date:",
                Location = new Point(20, y),
                Width = 120
            };

            // create date picker for the expense date
            var dtDate = new DateTimePicker
            {
                Location = new Point(150, y),
                Width = 200
            };

            editPanel.Controls.Add(lblDate);
            editPanel.Controls.Add(dtDate);
            y += 35;

            // create label for the expense category
            var lblCategory = new Label
            {
                Text = "Category:",
                Location = new Point(20, y),
                Width = 120
            };

            // create dropdown for the expense category
            var cboCategory = new ComboBox
            {
                Location = new Point(150, y),
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // populate the category dropdown with the categories from the budget 
            foreach (var c in budget.Categories)
                cboCategory.Items.Add(c.Name);

            // auto-select the current category of the expense being edited in the dropdown and if the category is not found, select the first category by default
            cboCategory.SelectedItem = category.Name;
            if (cboCategory.SelectedItem == null && cboCategory.Items.Count > 0)
                cboCategory.SelectedIndex = 0;

            // pre-fill the form fields with the existing values of the expense being edited
            txtDesc.Text = targetExpense.Description;
            numAmount.Value = (decimal)targetExpense.Amount;
            dtDate.Value = targetExpense.Date;

            editPanel.Controls.Add(lblCategory);
            editPanel.Controls.Add(cboCategory);
            y += 50;

            // create the button to save the changes to the expense
            var btnSave = new Button
            {
                Text = "Save",
                Location = new Point(180, y),
                Width = 100
            };
            BlueButton(btnSave);

            // handle the save button click event to update the existing expense based on the form inputs
            btnSave.Click += (_, __) =>
            {
                // get the selected category object based on the category name selected in the dropdown
                var newCategory = budget.Categories
                    .FirstOrDefault(c => c.Name == cboCategory.SelectedItem?.ToString());
                if (newCategory == null) return;

                // update fields
                targetExpense.Description = txtDesc.Text;
                targetExpense.Amount = (decimal)numAmount.Value;
                targetExpense.Date = dtDate.Value;

                // move category if changed
                if (category != newCategory)
                {
                    category.Expenses.Remove(targetExpense);
                    newCategory.Expenses.Add(targetExpense);
                }

                // save changes, hide the edit panel, and refresh the ListView and summary
                SaveBudgets();
                HideEditPanel();
                RenderBudgetSummary();
            };

            // create the button to cancel editing the expense
            var btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(460, y),
                Width = 100
            }; 
            GrayButton(btnCancel);

            // handle the cancel button click event to hide the edit panel and re-render the summary without making changes
            btnCancel.Click += (_, __) =>
            {
                HideEditPanel();
                RenderBudgetSummary();
            };

            // create the button to delete the expense
            var btnDelete = new Button
            {
                Text = "Delete",
                Location = new Point(320, y),
                Width = 100,
            };
            RedButton(btnDelete);

            // handle the delete button click event to confirm with the user and delete the expense if confirmed, then save changes and refresh the UI
            btnDelete.Click += (_, __) =>
            {
                var confirm = MessageBox.Show(this, "Are you sure you want to delete this expense?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    category.Expenses.Remove(targetExpense);
                    SaveBudgets();
                    HideEditPanel();
                    RenderBudgetSummary();
                }
            };

            // add all controls to the edit panel
            editPanel.Controls.Add(title);
            editPanel.Controls.Add(txtDesc);
            editPanel.Controls.Add(numAmount);
            editPanel.Controls.Add(dtDate);
            editPanel.Controls.Add(cboCategory);
            editPanel.Controls.Add(btnSave);
            editPanel.Controls.Add(btnDelete);
            editPanel.Controls.Add(btnCancel);
        }

        // helper to hide the edit panel and clear its controls (used after saving/canceling edits)
        private void HideEditPanel()
        {
            editPanel.Visible = false;
            editPanel.Controls.Clear();
            editPanel.SendToBack();
        }

        // helper to reload the entire UI (used after editing an expense to reset the view)
        private void ReloadUI()
        {
            Controls.Clear();
            BuildUI();
            LoadData();
            RenderBudgetSummary();
        }
    }
}