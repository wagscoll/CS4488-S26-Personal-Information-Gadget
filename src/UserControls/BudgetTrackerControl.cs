using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Demo_PIG_Tool.BudgetTool;
using BudgetDialogs;
using Demo_PIG_Tool.Manager;

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
        private Panel mainPanel = new Panel();
        private Label titleLabel = new Label();
        private ListView listViewSummary = new();
        private TextBox txtSummary = new();

        // constructor to set up the UI and load data
        public BudgetControl()
        {
            Dock = DockStyle.Fill;
            BuildUI();
            LoadData();
        }

        // builds the user interface with a header, top bar for budget selection and actions, and a split view for the list of expenses and budget summary
        private void BuildUI()
        {
            // make header panel
            mainPanel.BackColor = Color.FromArgb(240, 240, 240);
            mainPanel.Dock = DockStyle.Top;
            mainPanel.AutoSize = true;
            mainPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            mainPanel.Padding = new Padding(20, 20, 20, 10); // optional: bottom padding

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
            btnCreateBudget.Click += (_, __) => CreateBudgetDialog();
            BlueButton(btnCreateBudget);

            // button to add a new expense to the selected budget
            btnAddExpense.Text = "Add Expense";
            btnAddExpense.Click += (_, __) => AddExpenseDialog();
            BlueButton(btnAddExpense);

            // add the label, dropdown, and buttons to the top panel
            top.Controls.Add(lbl);
            top.Controls.Add(cboBudgets);
            top.Controls.Add(btnCreateBudget);
            top.Controls.Add(btnAddExpense);
   
            // make a split container for ListView and TextBox
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 350,
                BorderStyle = BorderStyle.None
            };

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
                new ColumnHeader { Text = "Category", Width = 200 },
                new ColumnHeader { Text = "Amount", Width = 200 },
                new ColumnHeader { Text = "Description", Width = 400 },
            });

            split.Panel1.Controls.Add(listViewSummary);

            // customize TextBox style for the summary
            txtSummary.Dock = DockStyle.Fill;
            txtSummary.Multiline = true;
            txtSummary.ScrollBars = ScrollBars.Vertical;
            txtSummary.ReadOnly = true;
            txtSummary.BackColor = Color.White;
            txtSummary.BorderStyle = BorderStyle.FixedSingle;

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


        // populate the ListView with all expenses for the selected budget and show a text summary of category totals
        private void RenderBudgetSummary()
        {
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
            foreach (var x in allExpenses)
            {
                var item = new ListViewItem(x.Expense.Date.ToShortDateString());
                item.SubItems.Add(x.Category);
                item.SubItems.Add(x.Expense.Amount.ToString("C"));
                item.SubItems.Add(x.Expense.Description);
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
            _allBudgets = _loadBudget.LoadBudgets();

            cboBudgets.Items.Clear();
            foreach (var budget in _allBudgets)
                cboBudgets.Items.Add(budget);

            if (cboBudgets.Items.Count > 0)
                cboBudgets.SelectedIndex = 0;
        }


        // refresh the dropdown with the current budgets and optionally select a specific budget
        private void RefreshBudgetDropdown(Budget? selectBudget = null)
        {
            cboBudgets.BeginUpdate();
            cboBudgets.DataSource = null;
            cboBudgets.DisplayMember = "";
            cboBudgets.DataSource = _allBudgets.ToList();
            cboBudgets.EndUpdate();

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
                SubToolManager.UpdateDocx(); // update the docx after saving budgets
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"Failed to save budgets.\n\n{ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
    }
}