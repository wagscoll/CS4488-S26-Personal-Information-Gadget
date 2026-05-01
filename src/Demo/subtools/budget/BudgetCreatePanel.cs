using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Author: Gabriel Ory
 * This is the Panel for creating a new budget with inputs for date, income,
 * and category budgets. It is used inside BudgetControl.
 */

namespace Demo_PIG_Tool.BudgetTool
{
    internal sealed class BudgetCreatePanel : Panel
    {
        // properties to get the values entered by the user
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

        // controls
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

        // buttons
        public Button BtnCreate { get; private set; }
        public Button BtnCancel { get; private set; }

        // constructor to setup the UI
        public BudgetCreatePanel()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.White;
            Padding = new Padding(20);

            // setup title
            var title = new Label
            {
                Text = "Create Budget",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            Controls.Add(title);

            // vertical position tracker for adding controls
            int y = 70;

            // helper function to add a label and control pair
            void Add(string text, Control c)
            {
                var lbl = new Label
                {
                    Text = text,
                    Location = new Point(20, y),
                    Width = 160
                };

                c.Location = new Point(200, y);
                c.Width = 200;

                Controls.Add(lbl);
                Controls.Add(c);

                y += 35;
            }

            // setup numeric controls for month, year, income, and category budgets
            SetupMonth(numMonth);
            SetupYear(numYear);
            SetupMoney(numIncome);
            SetupMoney(numSavings);
            SetupMoney(numInsurance);
            SetupMoney(numRent);
            SetupMoney(numGas);
            SetupMoney(numFood);
            SetupMoney(numFun);
            SetupMoney(numOther);

            // add the controls to the panel
            Add("Month (1–12)", numMonth);
            Add("Year", numYear);
            Add("Monthly Income", numIncome);

            y += 10;

            // section label for category budgets
            var section = new Label
            {
                Text = "Category Budgets",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(20, y),
                AutoSize = true
            };
            Controls.Add(section);
            y += 30;

            // add category budget controls
            Add("Savings", numSavings);
            Add("Insurance", numInsurance);
            Add("Rent/Mortgage", numRent);
            Add("Gas", numGas);
            Add("Food", numFood);
            Add("Fun", numFun);
            Add("Other", numOther);

            // button for creating the budget creation
            BtnCreate = new Button
            {
                Text = "Create",
                Width = 120,
                Location = new Point(180, y + 20)
            };
            BlueButton(BtnCreate);
            Controls.Add(BtnCreate);

            // button for canceling the budget creation
            BtnCancel = new Button
            {
                Text = "Cancel",
                Width = 120,
                Location = new Point(320, y + 20)
            };
            GrayButton(BtnCancel);
            Controls.Add(BtnCancel);
        }

        // helper methods to setup the numeric controls with appropriate ranges and formatting

        // month should be between 1 and 12, default to current month
        private static void SetupMonth(NumericUpDown n)
        {
            n.Minimum = 1;
            n.Maximum = 12;
            n.Value = DateTime.Now.Month;
        }

        // year should be between 2000 and 2100, default to current year
        private static void SetupYear(NumericUpDown n)
        {
            n.Minimum = 2000;
            n.Maximum = 2100;
            n.Value = DateTime.Now.Year;
        }

        // money controls should have 2 decimal places, a reasonable range, and a thousands separator
        private static void SetupMoney(NumericUpDown n)
        {
            n.DecimalPlaces = 2;
            n.Minimum = 0;
            n.Maximum = 10_000_000; 
            n.ThousandsSeparator = true;
            n.Increment = 50;
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
    }
}
