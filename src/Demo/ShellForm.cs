using Demo_PIG_Tool.Manager;

namespace Demo_PIG_Tool
{
    public class ShellForm : Form
    {
        private Panel sidebarPanel;
        private Panel contentPanel;

        private Button btnHome;
        private Button btnCalories;
        private Button btnTasks;
        private Button btnBudget;

        public ShellForm()
        {
            Text = "Personal Info Gadget";
            ClientSize = new System.Drawing.Size(1200, 703);
            MinimumSize = new System.Drawing.Size(800, 500);

            BuildContentArea();
            BuildSidebar();
            //NavigateTo("Home");
            NavigateTo("Calories");         //update to include tasks and budget when those are implemented
            //NavigateTo("TaskManager");
            //NavigateTo("BudgetTracker");
        }

        // Sidebar
        private void BuildSidebar()
        {
            sidebarPanel = new Panel();
            sidebarPanel.Dock = DockStyle.Left;
            sidebarPanel.Width = 160;
            sidebarPanel.BackColor = Color.FromArgb(30, 30, 30);

            Label appTitle = new Label();
            appTitle.Text = "PIG Tool";
            appTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            appTitle.ForeColor = Color.White;
            appTitle.Dock = DockStyle.Top;
            appTitle.Height = 60;
            appTitle.TextAlign = ContentAlignment.MiddleCenter;

            btnHome = new Button();
            btnHome.Text = "Home";
            btnHome.Dock = DockStyle.Top;
            btnHome.Height = 50;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.Font = new Font("Segoe UI", 10F);
            btnHome.ForeColor = Color.White;
            btnHome.BackColor = Color.FromArgb(30, 30, 30);
            btnHome.TextAlign = ContentAlignment.MiddleLeft;
            btnHome.Padding = new Padding(20, 0, 0, 0);
            btnHome.UseVisualStyleBackColor = false;
            btnHome.Click += new EventHandler(btnHome_Click);

            btnCalories = new Button();
            btnCalories.Text = "Calorie Tracker";
            btnCalories.Dock = DockStyle.Top;
            btnCalories.Height = 50;
            btnCalories.FlatStyle = FlatStyle.Flat;
            btnCalories.FlatAppearance.BorderSize = 0;
            btnCalories.Font = new Font("Segoe UI", 10F);
            btnCalories.ForeColor = Color.White;
            btnCalories.BackColor = Color.FromArgb(30, 30, 30);
            btnCalories.TextAlign = ContentAlignment.MiddleLeft;
            btnCalories.Padding = new Padding(20, 0, 0, 0);
            btnCalories.UseVisualStyleBackColor = false;
            btnCalories.Click += new EventHandler(btnCalories_Click);

            btnTasks = new Button();
            btnTasks.Text = "Task Manager";
            btnTasks.Dock = DockStyle.Top;
            btnTasks.Height = 50;
            btnTasks.FlatStyle = FlatStyle.Flat;
            btnTasks.FlatAppearance.BorderSize = 0;
            btnTasks.Font = new Font("Segoe UI", 10F);
            btnTasks.ForeColor = Color.White;
            btnTasks.BackColor = Color.FromArgb(30, 30, 30);
            btnTasks.TextAlign = ContentAlignment.MiddleLeft;
            btnTasks.Padding = new Padding(20, 0, 0, 0);
            btnTasks.UseVisualStyleBackColor = false;
            btnTasks.Click += new EventHandler(btnTasks_Click); //Placeholder for integrating Alex's task manager

            btnBudget = new Button();
            btnBudget.Text = "Budget Tracker";
            btnBudget.Dock = DockStyle.Top;
            btnBudget.Height = 50;
            btnBudget.FlatStyle = FlatStyle.Flat;
            btnBudget.FlatAppearance.BorderSize = 0;
            btnBudget.Font = new Font("Segoe UI", 10F);
            btnBudget.ForeColor = Color.White;
            btnBudget.BackColor = Color.FromArgb(30, 30, 30);
            btnBudget.TextAlign = ContentAlignment.MiddleLeft;
            btnBudget.Padding = new Padding(20, 0, 0, 0);
            btnBudget.UseVisualStyleBackColor = false;
            btnBudget.Click += new EventHandler(btnBudget_Click); //Placeholder for Gabriel's budget tracker

            sidebarPanel.Controls.Add(btnBudget);
            sidebarPanel.Controls.Add(btnTasks);
            sidebarPanel.Controls.Add(btnCalories);
            sidebarPanel.Controls.Add(btnHome);
            sidebarPanel.Controls.Add(appTitle);

            Controls.Add(sidebarPanel);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            NavigateTo("Home");
        }

        private void btnCalories_Click(object sender, EventArgs e)
        {
            NavigateTo("Calories");
        }

        private void btnTasks_Click(object sender, EventArgs e)
        {
            NavigateTo("Tasks");
        }

        private void btnBudget_Click(object sender, EventArgs e)
        {
            NavigateTo("Budget");
        }

        //  Content area 
        private void BuildContentArea()
        {
            contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.FromArgb(245, 245, 245);
            Controls.Add(contentPanel);
        }

        //  Navigation 
        private void NavigateTo(string section)
        {
            SubToolManager.UpdateDocx();


            contentPanel.Controls.Clear();

            Control panel;

            if (section == "Calories")
            {
                panel = new CalorieTrackerControl();
            }

            /*      
                    ~~~ INPUT TAKS SCREEN HERE ~~~
    
            else if (section == "Tasks")
            {
                panel = new ...
            }
            */





            /*                            
                    ~~~ Input Budget Screen Here ~~~

            else if (section == "Budget")
            { 
                panel = new ...
            }

            */

            else
            {
                Label placeholder = new Label();
                placeholder.Text = section + "\n\nComing soon...";
                placeholder.Font = new Font("Segoe UI", 14F);
                placeholder.ForeColor = Color.FromArgb(150, 150, 150);
                placeholder.TextAlign = ContentAlignment.MiddleCenter;
                panel = placeholder;
            }

            panel.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(panel);

            UpdateSidebarHighlight(section);
        }

        private void UpdateSidebarHighlight(string active)
        {
            btnHome.BackColor = Color.FromArgb(30, 30, 30);
            btnCalories.BackColor = Color.FromArgb(30, 30, 30);
            btnTasks.BackColor = Color.FromArgb(30, 30, 30);
            btnBudget.BackColor = Color.FromArgb(30, 30, 30);

            if (active == "Home")
                btnHome.BackColor = Color.FromArgb(0, 120, 215);
            else if (active == "Calories")
                btnCalories.BackColor = Color.FromArgb(0, 120, 215);
            else if (active == "Tasks")
                btnTasks.BackColor = Color.FromArgb(0, 120, 215);
            else if (active == "Budget")
                btnBudget.BackColor = Color.FromArgb(0, 120, 215);
        }
    }
}