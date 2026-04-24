using System;
using System.Drawing;
using System.Windows.Forms;

/* Author: Gabriel Ory
 * This file contains the code for creating dialogs for the Budget and Shopping sub-tools.
 */

namespace BudgetDialogs
{
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