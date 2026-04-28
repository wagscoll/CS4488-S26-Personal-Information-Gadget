using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace Demo_PIG_Tool
{
    public class HomeControl : UserControl
    {
        // This control serves as the home/dashboard view of the Personal Information Gadget.
        public HomeControl()
        {
            Dock = DockStyle.Fill;
            BackColor = Color.FromArgb(255, 255, 255);
            BuildUI();
        }

        private void BuildUI()
        {
            // Main panel with some padding and vertical flow layout
            var mainPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            //Add a title and subtitle to the dashboard
            var titleLabel = new Label
            {
                Text = "Welcome to Your Personal Information Gadget",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 30),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 20)
            };

            var subtitleLabel = new Label
            {
                Text = "Track your health, tasks, budget, and shopping all in one place!",
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                ForeColor = Color.FromArgb(80, 80, 80),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 40)
            };

            // Add date label to show current date
            var dateLabel = new Label
            {
                Text = $"Today: {DateTime.Now:MMMM dd, yyyy}",
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = Color.FromArgb(0, 120, 215),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 20)
            };

            // Create a grid panel to hold the summary cards from the different subtools
            var cardPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 2,
                Dock = DockStyle.Fill,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 20),
                Width = 900,
                Height = 400
            };

            // Set column and row styles to evenly distribute the cards
            cardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            cardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            cardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            cardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            //Get the counts to display them on the cards
            int taskCount = GetTaskCount();
            int budgetCount = GetBudgetCount();
            int shoppingCount = GetShoppingCount();
            int healthCount = GetHealthCount();
            int projectCount = GetProjectCount();

            // Add the four cards to the screen and the other UI elements to the main panel
            cardPanel.Controls.Add(CreateCard("Calorie Tracker", "You have", healthCount, "health entries"), 0, 0);
            cardPanel.Controls.Add(CreateCard("Task Manager", "You have", projectCount, $"projects and {taskCount} tasks"), 1, 0);
            cardPanel.Controls.Add(CreateCard("Budget Tracker", "You have logged", budgetCount, "budget expenses"), 0, 1);
            cardPanel.Controls.Add(CreateCard("Shopping List", "You have", shoppingCount, "shopping lists"), 1, 1);
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(subtitleLabel);
            mainPanel.Controls.Add(dateLabel);
            mainPanel.Controls.Add(cardPanel);

            Controls.Add(mainPanel);
        }

        // Helper function that creates a card from a title, some text, a big number, and some more text. This is used to create the summary cards for each subtool on the dashboard.
        //The top text and bottom text are a stylistic choice to make the cards look nicer and more consistent, but they could be removed if desired.
        private Panel CreateCard(string title, string topText, int bigNumber, string bottomText)
        {
            var card = new Panel
            {
                Width = 400,
                Height = 180,
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(15)
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 20));

            layout.Controls.Add(MakeCenteredLabel(title, 14, FontStyle.Bold), 0, 0);
            layout.Controls.Add(MakeCenteredLabel(topText, 11, FontStyle.Regular), 0, 1);
            layout.Controls.Add(MakeCenteredLabel(bigNumber.ToString(), 32, FontStyle.Bold), 0, 2);
            layout.Controls.Add(MakeCenteredLabel(bottomText, 11, FontStyle.Regular), 0, 3);

            card.Controls.Add(layout);
            return card;
        }

        // Helper function to create a label with centered text, used for the text in the cards.
        private Label MakeCenteredLabel(string text, int size, FontStyle style)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", size, style),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = false
            };
        }

        //This helper function gets the root path to the logs folder, used when we pull the .txt files to get counts
        private string GetRootLogsPath()
        {
            string rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "logs"));
            return rootPath;
        }

        // The following functions read the .txt log files for each subtool and count the number of entries to display on the dashboard cards.
        // They look for specific formatting depending on each one of the logs to determine what counts as a valid entry.
        private int GetTaskCount()
        {
            string tasksPath = Path.Combine(GetRootLogsPath(), "projectsAndTasksLogs.txt");
            if (File.Exists(tasksPath))
            {
                return File.ReadAllLines(tasksPath).Count(line => !string.IsNullOrWhiteSpace(line) && line.StartsWith("TASK"));
            }
            return 0;
        }

        private int GetProjectCount()
        {
            string tasksPath = Path.Combine(GetRootLogsPath(), "projectsAndTasksLogs.txt");
            if (File.Exists(tasksPath))
            {
                return File.ReadAllLines(tasksPath).Count(line => !string.IsNullOrWhiteSpace(line) && line.StartsWith("PROJECT"));
            }
            return 0;
        }

        private int GetHealthCount()
        {
            string healthPath = Path.Combine(GetRootLogsPath(), "healthlogs.txt");
            if (File.Exists(healthPath))
            {
                return File.ReadAllLines(healthPath).Count(line => !string.IsNullOrWhiteSpace(line));
            }
            return 0;
        }

        private int GetBudgetCount()
        {
            string budgetPath = Path.Combine(GetRootLogsPath(), "budgetLogs.txt");
            if (File.Exists(budgetPath))
            {
                return File.ReadAllLines(budgetPath).Count(line => !string.IsNullOrWhiteSpace(line) && line.StartsWith ("- "));
            }
            return 0;
        }

        private int GetShoppingCount()
        {
            string shoppingPath = Path.Combine(GetRootLogsPath(), "shoppingLogs.txt");
            if (File.Exists(shoppingPath))
            {
                return File.ReadAllLines(shoppingPath).Count(line => !string.IsNullOrWhiteSpace(line) && line.StartsWith("Shopping"));
            }
            return 0;
        }
    }
}