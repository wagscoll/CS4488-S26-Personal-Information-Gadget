using Demo_PIG_Tool.Utils;
using Demo_PIG_Tool.Manager;

namespace ProjectAndTaskManagerFormApp
{
    public partial class Form1 : Form
    {
        private List<UtilsProject> projects = new();
        private List<UtilsTask> tasks = new();

        public Form1()
        {
            InitializeComponent();
            loadData();
            RefreshListView();
        }
        // anh 3/2 - Refreshes the list view panel and toggles visibility of panels
        //           and button colors to indicate which tab the user is on when the "View/Edit" button is clicked.
        private void ViewEditButton_Click(object sender, EventArgs e)
        {
            RefreshListView();
            listView.Visible = true;
            editPanel.Visible = false;
            schedulePanel.Visible = false;
            viewEditButton.BackColor = Color.FromArgb(0, 20, 215);
            createTaskButton.BackColor = Color.FromArgb(0, 120, 215);
            createProjectButton.BackColor = Color.FromArgb(0, 120, 215);
            twoWeeksButton.BackColor = Color.FromArgb(0, 120, 215);
            editTipLabel.Visible = true;
        }
        // anh 3/2 - Displays the form to create a new task and toggles visibility of panels
        private void CreateTaskButton_Click(object sender, EventArgs e)
        {
            ShowCreateTaskForm();
            viewEditButton.BackColor = Color.FromArgb(0, 120, 215);
            createTaskButton.BackColor = Color.FromArgb(0, 20, 215);
            createProjectButton.BackColor = Color.FromArgb(0, 120, 215);
            twoWeeksButton.BackColor = Color.FromArgb(0, 120, 215);
            editTipLabel.Visible = false;
        }
        // anh 3/2 - Displays the form to create a new project and toggles visibility of panels
        private void CreateProjectButton_Click(object sender, EventArgs e)
        {
            ShowCreateProjectForm();
            viewEditButton.BackColor = Color.FromArgb(0, 120, 215);
            createTaskButton.BackColor = Color.FromArgb(0, 120, 215);
            createProjectButton.BackColor = Color.FromArgb(0, 20, 215);
            twoWeeksButton.BackColor = Color.FromArgb(0, 120, 215);
            editTipLabel.Visible = false;
        }
        // anh 3/2 - Displays the form to edit a task or project when
        //           an item in the list view is double-clicked and toggles visibility of panels
        private void ListView_DoubleClick(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                var selectedItem = listView.SelectedItems[0];
                int id = int.Parse(selectedItem.SubItems[0].Text);
                string type = selectedItem.SubItems[1].Text;

                if (type == "PROJECT")
                {
                    ShowEditProjectForm(id);
                }
                else if (type == "TASK")
                {
                    ShowEditTaskForm(id);
                }
            }
        }
        // anh 3/2 - Refreshes the list view with the latest projects and tasks data, and toggles visibility of panels
        private void RefreshListView()
        {
            listView.Items.Clear();

            foreach (var project in projects)
            {
                var item = new ListViewItem(project.GetProjectId().ToString());
                item.SubItems.Add("PROJECT");
                item.SubItems.Add(project.GetProjectName());
                item.SubItems.Add(project.getisImportant() ? "Yes" : "No");
                item.SubItems.Add(project.getisUrgent() ? "Yes" : "No");
                item.SubItems.Add(project.getDueDate().ToString("yyyy-MM-dd HH:mm"));
                item.SubItems.Add(project.getEstimatedHours().ToString());
                item.SubItems.Add("-");
                listView.Items.Add(item);
            }

            foreach (var task in tasks)
            {
                var item = new ListViewItem(task.GetTaskId().ToString());
                item.SubItems.Add("TASK");
                item.SubItems.Add(task.GetTaskName());
                item.SubItems.Add(task.getisImportant() ? "Yes" : "No");
                item.SubItems.Add(task.getisUrgent() ? "Yes" : "No");
                item.SubItems.Add(task.getDueDate().ToString("yyyy-MM-dd HH:mm"));
                item.SubItems.Add(task.getEstimatedHours().ToString());
                item.SubItems.Add(task.getProjectId() == -1 ? "-" : task.getProjectId().ToString());
                listView.Items.Add(item);
            }
        }
        // anh 3/2 - Displays the form to create a new task with input fields for task attributes and toggles visibility of panels
        private void ShowCreateTaskForm()
        {
            editPanel.Controls.Clear();
            editPanel.Visible = true;
            listView.Visible = false;
            schedulePanel.Visible = false;

            int yPos = 20;

            Label titleLabel = new Label { Text = "Create New Task", Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(20, yPos), AutoSize = true };
            editPanel.Controls.Add(titleLabel);
            yPos += 40;

            Label nameLabel = new Label { Text = "Task Name:", Location = new Point(20, yPos), Width = 150 };
            TextBox nameTextBox = new TextBox { Location = new Point(180, yPos), Width = 300, Name = "taskName" };
            editPanel.Controls.Add(nameLabel);
            editPanel.Controls.Add(nameTextBox);
            yPos += 35;

            Label importantLabel = new Label { Text = "Important:", Location = new Point(20, yPos), Width = 150 };
            CheckBox importantCheckBox = new CheckBox { Location = new Point(180, yPos), Name = "isImportant" };
            editPanel.Controls.Add(importantLabel);
            editPanel.Controls.Add(importantCheckBox);
            yPos += 35;

            Label urgentLabel = new Label { Text = "Urgent:", Location = new Point(20, yPos), Width = 150 };
            CheckBox urgentCheckBox = new CheckBox { Location = new Point(180, yPos), Name = "isUrgent" };
            editPanel.Controls.Add(urgentLabel);
            editPanel.Controls.Add(urgentCheckBox);
            yPos += 35;

            Label dueDateLabel = new Label { Text = "Due Date:", Location = new Point(20, yPos), Width = 150 };
            DateTimePicker dueDatePicker = new DateTimePicker { Location = new Point(180, yPos), Width = 300, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm", Name = "dueDate" };
            editPanel.Controls.Add(dueDateLabel);
            editPanel.Controls.Add(dueDatePicker);
            yPos += 35;

            Label hoursLabel = new Label { Text = "Estimated Hours:", Location = new Point(20, yPos), Width = 150 };
            NumericUpDown hoursNumeric = new NumericUpDown { Location = new Point(180, yPos), Width = 100, DecimalPlaces = 1, Maximum = 1000, Name = "hours" };
            editPanel.Controls.Add(hoursLabel);
            editPanel.Controls.Add(hoursNumeric);
            yPos += 35;

            Label projectLabel = new Label { Text = "Part of Project:", Location = new Point(20, yPos), Width = 150 };
            ComboBox projectCombo = new ComboBox { Location = new Point(180, yPos), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, Name = "projectId" };
            projectCombo.Items.Add("No Project");
            projectCombo.Tag = -1;
            foreach (var project in projects)
            {
                projectCombo.Items.Add(project.GetProjectName());
            }
            projectCombo.SelectedIndex = 0;
            editPanel.Controls.Add(projectLabel);
            editPanel.Controls.Add(projectCombo);
            yPos += 50;

            Button saveButton = new Button { Text = "Create Task", Location = new Point(180, yPos), Width = 120, Height = 35, BackColor = Color.FromArgb(0, 120, 215), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            saveButton.Click += (s, e) =>
            {
                string taskName = nameTextBox.Text;
                bool isImportant = importantCheckBox.Checked;
                bool isUrgent = urgentCheckBox.Checked;
                DateTime dueDate = dueDatePicker.Value;
                float hours = (float)hoursNumeric.Value;
                int projId = -1;

                if (projectCombo.SelectedIndex > 0)
                {
                    projId = projects[projectCombo.SelectedIndex - 1].GetProjectId();
                }

                if (string.IsNullOrWhiteSpace(taskName))
                {
                    MessageBox.Show("Please enter a task name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                tasks.Add(new UtilsTask(freshId("task"), taskName, isImportant, isUrgent, dueDate, hours, projId));
                saveChanges();
                MessageBox.Show("Task created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshListView();
                listView.Visible = true;
                editPanel.Visible = false;
                schedulePanel.Visible = false;
            };
            editPanel.Controls.Add(saveButton);

            Button cancelButton = new Button { Text = "Cancel", Location = new Point(310, yPos), Width = 100, Height = 35, BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            cancelButton.Click += (s, e) =>
            {
                listView.Visible = true;
                editPanel.Visible = false;
                schedulePanel.Visible = false;
            };
            editPanel.Controls.Add(cancelButton);
        }
        // anh 3/2 - Displays the form to create a new project with input fields for project attributes and toggles visibility of panels
        private void ShowCreateProjectForm()
        {
            editPanel.Controls.Clear();
            editPanel.Visible = true;
            schedulePanel.Visible = false;
            listView.Visible = false;

            int yPos = 20;

            Label titleLabel = new Label { Text = "Create New Project", Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(20, yPos), AutoSize = true };
            editPanel.Controls.Add(titleLabel);
            yPos += 40;

            Label nameLabel = new Label { Text = "Project Name:", Location = new Point(20, yPos), Width = 150 };
            TextBox nameTextBox = new TextBox { Location = new Point(180, yPos), Width = 300, Name = "projectName" };
            editPanel.Controls.Add(nameLabel);
            editPanel.Controls.Add(nameTextBox);
            yPos += 35;

            Label importantLabel = new Label { Text = "Important:", Location = new Point(20, yPos), Width = 150 };
            CheckBox importantCheckBox = new CheckBox { Location = new Point(180, yPos), Name = "isImportant" };
            editPanel.Controls.Add(importantLabel);
            editPanel.Controls.Add(importantCheckBox);
            yPos += 35;

            Label urgentLabel = new Label { Text = "Urgent:", Location = new Point(20, yPos), Width = 150 };
            CheckBox urgentCheckBox = new CheckBox { Location = new Point(180, yPos), Name = "isUrgent" };
            editPanel.Controls.Add(urgentLabel);
            editPanel.Controls.Add(urgentCheckBox);
            yPos += 35;

            Label dueDateLabel = new Label { Text = "Due Date:", Location = new Point(20, yPos), Width = 150 };
            DateTimePicker dueDatePicker = new DateTimePicker { Location = new Point(180, yPos), Width = 300, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm", Name = "dueDate" };
            editPanel.Controls.Add(dueDateLabel);
            editPanel.Controls.Add(dueDatePicker);
            yPos += 35;

            Label hoursLabel = new Label { Text = "Estimated Hours:", Location = new Point(20, yPos), Width = 150 };
            NumericUpDown hoursNumeric = new NumericUpDown { Location = new Point(180, yPos), Width = 100, DecimalPlaces = 1, Maximum = 1000, Name = "hours" };
            editPanel.Controls.Add(hoursLabel);
            editPanel.Controls.Add(hoursNumeric);
            yPos += 50;

            Button saveButton = new Button { Text = "Create Project", Location = new Point(180, yPos), Width = 120, Height = 35, BackColor = Color.FromArgb(0, 120, 215), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            saveButton.Click += (s, e) =>
            {
                string projectName = nameTextBox.Text;
                bool isImportant = importantCheckBox.Checked;
                bool isUrgent = urgentCheckBox.Checked;
                DateTime dueDate = dueDatePicker.Value;
                float hours = (float)hoursNumeric.Value;

                if (string.IsNullOrWhiteSpace(projectName))
                {
                    MessageBox.Show("Please enter a project name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                projects.Add(new UtilsProject(freshId("project"), projectName, isImportant, isUrgent, dueDate, hours));
                saveChanges();
                MessageBox.Show("Project created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshListView();
                listView.Visible = true;
                editPanel.Visible = false;
                schedulePanel.Visible = false;
            };
            editPanel.Controls.Add(saveButton);

            Button cancelButton = new Button { Text = "Cancel", Location = new Point(310, yPos), Width = 100, Height = 35, BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            cancelButton.Click += (s, e) =>
            {
                listView.Visible = true;
                editPanel.Visible = false;
                schedulePanel.Visible = false;
            };
            editPanel.Controls.Add(cancelButton);
        }
        // anh 3/2 - Displays the form to edit an existing task with pre-filled input
        //           fields for task attributes and toggles visibility of panels
        private void ShowEditTaskForm(int taskId)
        {
            UtilsTask taskToEdit = tasks.FirstOrDefault(t => t.GetTaskId() == taskId);
            if (taskToEdit == null) return;

            editPanel.Controls.Clear();
            editPanel.Visible = true;
            listView.Visible = false;
            schedulePanel.Visible = false;

            int yPos = 20;

            Label titleLabel = new Label { Text = $"Edit Task: {taskToEdit.GetTaskName()}", Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(20, yPos), AutoSize = true };
            editPanel.Controls.Add(titleLabel);
            yPos += 40;

            Label nameLabel = new Label { Text = "Task Name:", Location = new Point(20, yPos), Width = 150 };
            TextBox nameTextBox = new TextBox { Location = new Point(180, yPos), Width = 300, Text = taskToEdit.GetTaskName() };
            editPanel.Controls.Add(nameLabel);
            editPanel.Controls.Add(nameTextBox);
            yPos += 35;

            Label importantLabel = new Label { Text = "Important:", Location = new Point(20, yPos), Width = 150 };
            CheckBox importantCheckBox = new CheckBox { Location = new Point(180, yPos), Checked = taskToEdit.getisImportant() };
            editPanel.Controls.Add(importantLabel);
            editPanel.Controls.Add(importantCheckBox);
            yPos += 35;

            Label urgentLabel = new Label { Text = "Urgent:", Location = new Point(20, yPos), Width = 150 };
            CheckBox urgentCheckBox = new CheckBox { Location = new Point(180, yPos), Checked = taskToEdit.getisUrgent() };
            editPanel.Controls.Add(urgentLabel);
            editPanel.Controls.Add(urgentCheckBox);
            yPos += 35;

            Label dueDateLabel = new Label { Text = "Due Date:", Location = new Point(20, yPos), Width = 150 };
            DateTimePicker dueDatePicker = new DateTimePicker { Location = new Point(180, yPos), Width = 300, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm", Value = taskToEdit.getDueDate() };
            editPanel.Controls.Add(dueDateLabel);
            editPanel.Controls.Add(dueDatePicker);
            yPos += 35;

            Label hoursLabel = new Label { Text = "Estimated Hours:", Location = new Point(20, yPos), Width = 150 };
            NumericUpDown hoursNumeric = new NumericUpDown { Location = new Point(180, yPos), Width = 100, DecimalPlaces = 1, Maximum = 1000, Value = (decimal)taskToEdit.getEstimatedHours() };
            editPanel.Controls.Add(hoursLabel);
            editPanel.Controls.Add(hoursNumeric);
            yPos += 35;

            Label projectLabel = new Label { Text = "Part of Project:", Location = new Point(20, yPos), Width = 150 };
            ComboBox projectCombo = new ComboBox { Location = new Point(180, yPos), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            projectCombo.Items.Add("No Project");
            int selectedIndex = 0;
            for (int i = 0; i < projects.Count; i++)
            {
                projectCombo.Items.Add(projects[i].GetProjectName());
                if (projects[i].GetProjectId() == taskToEdit.getProjectId())
                {
                    selectedIndex = i + 1;
                }
            }
            projectCombo.SelectedIndex = selectedIndex;
            editPanel.Controls.Add(projectLabel);
            editPanel.Controls.Add(projectCombo);
            yPos += 50;

            Button saveButton = new Button { Text = "Save Changes", Location = new Point(180, yPos), Width = 120, Height = 35, BackColor = Color.FromArgb(0, 120, 215), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            saveButton.Click += (s, e) =>
            {
                taskToEdit.updateTaskName(nameTextBox.Text);
                taskToEdit.updateIsImportant(importantCheckBox.Checked);
                taskToEdit.updateIsUrgent(urgentCheckBox.Checked);
                taskToEdit.updateDueDate(dueDatePicker.Value);
                taskToEdit.updateEstimatedHours((float)hoursNumeric.Value);

                int projId = -1;
                if (projectCombo.SelectedIndex > 0)
                {
                    projId = projects[projectCombo.SelectedIndex - 1].GetProjectId();
                }
                taskToEdit.updateProjectId(projId);

                saveChanges();
                MessageBox.Show("Task updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshListView();
                listView.Visible = true;
                editPanel.Visible = false;
                schedulePanel.Visible = false;
            };
            editPanel.Controls.Add(saveButton);

            Button deleteButton = new Button { Text = "Delete", Location = new Point(310, yPos), Width = 100, Height = 35, BackColor = Color.Red, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            deleteButton.Click += (s, e) =>
            {
                var result = MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    tasks.Remove(taskToEdit);
                    saveChanges();
                    MessageBox.Show("Task deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshListView();
                    listView.Visible = true;
                    editPanel.Visible = false;
                    schedulePanel.Visible = false;
                }
            };
            editPanel.Controls.Add(deleteButton);

            Button cancelButton = new Button { Text = "Cancel", Location = new Point(420, yPos), Width = 100, Height = 35, BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            cancelButton.Click += (s, e) =>
            {
                listView.Visible = true;
                editPanel.Visible = false;
                schedulePanel.Visible = false;
            };
            editPanel.Controls.Add(cancelButton);
        }
        // anh 3/2 - Displays the form to edit an existing project with pre-filled input
        private void ShowEditProjectForm(int projectId)
        {
            UtilsProject projectToEdit = projects.FirstOrDefault(p => p.GetProjectId() == projectId);
            if (projectToEdit == null) return;

            editPanel.Controls.Clear();
            editPanel.Visible = true;
            listView.Visible = false;
            schedulePanel.Visible = false;

            int yPos = 20;

            Label titleLabel = new Label { Text = $"Edit Project: {projectToEdit.GetProjectName()}", Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(20, yPos), AutoSize = true };
            editPanel.Controls.Add(titleLabel);
            yPos += 40;

            Label nameLabel = new Label { Text = "Project Name:", Location = new Point(20, yPos), Width = 150 };
            TextBox nameTextBox = new TextBox { Location = new Point(180, yPos), Width = 300, Text = projectToEdit.GetProjectName() };
            editPanel.Controls.Add(nameLabel);
            editPanel.Controls.Add(nameTextBox);
            yPos += 35;

            Label importantLabel = new Label { Text = "Important:", Location = new Point(20, yPos), Width = 150 };
            CheckBox importantCheckBox = new CheckBox { Location = new Point(180, yPos), Checked = projectToEdit.getisImportant() };
            editPanel.Controls.Add(importantLabel);
            editPanel.Controls.Add(importantCheckBox);
            yPos += 35;

            Label urgentLabel = new Label { Text = "Urgent:", Location = new Point(20, yPos), Width = 150 };
            CheckBox urgentCheckBox = new CheckBox { Location = new Point(180, yPos), Checked = projectToEdit.getisUrgent() };
            editPanel.Controls.Add(urgentLabel);
            editPanel.Controls.Add(urgentCheckBox);
            yPos += 35;

            Label dueDateLabel = new Label { Text = "Due Date:", Location = new Point(20, yPos), Width = 150 };
            DateTimePicker dueDatePicker = new DateTimePicker { Location = new Point(180, yPos), Width = 300, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd HH:mm", Value = projectToEdit.getDueDate() };
            editPanel.Controls.Add(dueDateLabel);
            editPanel.Controls.Add(dueDatePicker);
            yPos += 35;

            Label hoursLabel = new Label { Text = "Estimated Hours:", Location = new Point(20, yPos), Width = 150 };
            NumericUpDown hoursNumeric = new NumericUpDown { Location = new Point(180, yPos), Width = 100, DecimalPlaces = 1, Maximum = 1000, Value = (decimal)projectToEdit.getEstimatedHours() };
            editPanel.Controls.Add(hoursLabel);
            editPanel.Controls.Add(hoursNumeric);
            yPos += 50;

            Button saveButton = new Button { Text = "Save Changes", Location = new Point(180, yPos), Width = 120, Height = 35, BackColor = Color.FromArgb(0, 120, 215), ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            saveButton.Click += (s, e) =>
            {
                projectToEdit.updateName(nameTextBox.Text);
                projectToEdit.updateIsImportant(importantCheckBox.Checked);
                projectToEdit.updateIsUrgent(urgentCheckBox.Checked);
                projectToEdit.updateDueDate(dueDatePicker.Value);
                projectToEdit.updateEstimatedHours((float)hoursNumeric.Value);

                saveChanges();
                MessageBox.Show("Project updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshListView();
                listView.Visible = true;
                editPanel.Visible = false;
                schedulePanel.Visible = false;
            };
            editPanel.Controls.Add(saveButton);

            Button deleteButton = new Button { Text = "Delete", Location = new Point(310, yPos), Width = 100, Height = 35, BackColor = Color.Red, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            deleteButton.Click += (s, e) =>
            {
                var relatedTasks = tasks.Where(t => t.getProjectId() == projectId).ToList();
                string message = relatedTasks.Count > 0
                    ? $"This project has {relatedTasks.Count} related task(s). Delete all related tasks as well?"
                    : "Are you sure you want to delete this project?";

                var result = MessageBox.Show(message, "Confirm Delete", relatedTasks.Count > 0 ? MessageBoxButtons.YesNoCancel : MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    foreach (var task in relatedTasks)
                    {
                        tasks.Remove(task);
                    }
                    projects.Remove(projectToEdit);
                    saveChanges();
                    MessageBox.Show("Project deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshListView();
                    listView.Visible = true;
                    editPanel.Visible = false;
                    schedulePanel.Visible = false;
                }
                else if (result == DialogResult.No && relatedTasks.Count > 0)
                {
                    foreach (var task in relatedTasks)
                    {
                        task.updateProjectId(-1);
                    }
                    projects.Remove(projectToEdit);
                    saveChanges();
                    MessageBox.Show("Project deleted successfully! Related tasks are now independent.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshListView();
                    listView.Visible = true;
                    editPanel.Visible = false;
                    schedulePanel.Visible = false;
                }
            };
            editPanel.Controls.Add(deleteButton);

            Button cancelButton = new Button { Text = "Cancel", Location = new Point(420, yPos), Width = 100, Height = 35, BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            cancelButton.Click += (s, e) =>
            {
                listView.Visible = true;
                editPanel.Visible = false;
                schedulePanel.Visible = false;
            };
            editPanel.Controls.Add(cancelButton);
        }
        // anh 3/2 - Helper method to get the file path for storing projects and tasks logs,
        //           ensuring it's in a consistent location relative to the application's base directory.
        private string GetProjectsAndTasksLogPath()
        {
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "logs", "projectsAndTasksLogs.txt"));
        }
        // anh 3/2 - Loads projects and tasks data from a log file,
        //           parsing each line to create corresponding objects and populate the lists.
        private void loadData()
        {
            string path = GetProjectsAndTasksLogPath();
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        string[] entries = s.Split('|');
                        if (entries[0] == "PROJECT")
                        {
                            projects.Add(new UtilsProject(
                                int.Parse(entries[1]),
                                entries[2],
                                bool.Parse(entries[3]),
                                bool.Parse(entries[4]),
                                DateTime.Parse(entries[5]),
                                float.Parse(entries[6])
                            ));
                        }
                        else if (entries[0] == "TASK")
                        {
                            tasks.Add(new UtilsTask(
                                int.Parse(entries[1]),
                                entries[2],
                                bool.Parse(entries[3]),
                                bool.Parse(entries[4]),
                                DateTime.Parse(entries[5]),
                                float.Parse(entries[6]),
                                int.Parse(entries[7])
                            ));
                        }
                    }
                }
            }
        }
        // anh 3/2 - Generates a unique ID for a new project or task by checking existing IDs in the respective list,
        private int freshId(string type)
        {
            int id = 1;
            if (type == "project")
            {
                bool freshId = false;
                while (!freshId)
                {
                    freshId = true;
                    foreach (UtilsProject project in projects)
                    {
                        if (id == project.GetProjectId())
                        {
                            freshId = false;
                            break;
                        }
                    }
                    if (freshId)
                    {
                        return id;
                    }
                    else
                    {
                        id++;
                    }
                }
            }
            else
            {
                bool freshId = false;
                while (!freshId)
                {
                    freshId = true;
                    foreach (UtilsTask t in tasks)
                    {
                        if (id == t.GetTaskId())
                        {
                            freshId = false;
                            break;
                        }
                    }
                    if (freshId)
                    {
                        return id;
                    }
                    else
                    {
                        id++;
                    }
                }
            }
            return -1;
        }
        // anh 3/2 - Saves the current state of projects and tasks to a log file
        private void saveChanges()
        {
            string path = GetProjectsAndTasksLogPath();
            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (UtilsProject project in projects)
                {
                    sw.WriteLine($"PROJECT|{project.GetProjectId()}|{project.GetProjectName()}|{project.getisImportant()}|{project.getisUrgent()}|{project.getDueDate()}|{project.getEstimatedHours()}");
                }
                foreach (UtilsTask task in tasks)
                {
                    sw.WriteLine($"TASK|{task.GetTaskId()}|{task.GetTaskName()}|{task.getisImportant()}|{task.getisUrgent()}|{task.getDueDate()}|{task.getEstimatedHours()}|{task.getProjectId()}");
                }
            }
        }

        private void twoWeeksButton_Click(object sender, EventArgs e)
        {
            // Build and display the schedule text
            scheduleTextBox.Text = BuildScheduleText(14);

            // toggle what's visible
            schedulePanel.Visible = true;
            listView.Visible = false;
            editPanel.Visible = false;

            editTipLabel.Visible = false;
            //Change the button colors to display what tab the user is on -ANH 3/5/26
            viewEditButton.BackColor = Color.FromArgb(0, 120, 215);
            createTaskButton.BackColor = Color.FromArgb(0, 120, 215);
            createProjectButton.BackColor = Color.FromArgb(0, 120, 215);
            twoWeeksButton.BackColor = Color.FromArgb(0, 20, 215);
        }

        // Displays tasks and projects due in the next n days (default 14)
        // In the WinForms version, return the formatted text so it can go in a textbox.
        private string BuildScheduleText(int n = 14)
        {
            // Build output instead of Console.WriteLine
            var sb = new System.Text.StringBuilder();

            // Get today's date and calculate the end date
            // We add n+1 days to include tasks and projects due on the nth day
            DateTime today = DateTime.Today;
            DateTime endDate = today.AddDays(n + 1);

            // Filter tasks and projects that are due between today and the end date    
            // We use >= today to include tasks/projects due today, 
            // and < endDate to exclude those due on the day after the nth day
            var tasksDueInTwoWeeks = tasks
                .Where(task => task.getDueDate() >= today && task.getDueDate() < endDate)
                .ToList();

            var projectsDueInTwoWeeks = projects
                .Where(project => project.getDueDate() >= today && project.getDueDate() < endDate)
                .ToList();

            // Group projects by their due date
            var groupedProjects = projectsDueInTwoWeeks
                .GroupBy(project => project.getDueDate().Date)
                .ToDictionary(g => g.Key, g => g.ToList());

            sb.AppendLine($"Tasks due in the next {n} days:\n");

            // Group tasks by their due date aswell
            var groupedTasksDict = tasksDueInTwoWeeks
                .GroupBy(task => task.getDueDate().Date)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Get a sorted list of all unique due dates from both tasks and projects
            // We use Union to combine the keys from both dictionaries, and OrderBy to sort them
            var allDates = groupedTasksDict.Keys
                .Union(groupedProjects.Keys)
                .OrderBy(d => d)
                .ToList();

            //no dates
            if (allDates.Count == 0)
            {
                sb.AppendLine("(No tasks or projects due in this range.)\n");
                return sb.ToString();
            }

            int dayCounter = 1;
            // Iterate through each date and display the tasks and projects due on that date
            foreach (var date in allDates)
            {
                sb.AppendLine($"{dayCounter}) {date:ddd MMM dd, yyyy}");
                // First display tasks due on that date
                if (groupedTasksDict.TryGetValue(date, out var tasksOnThatDay))
                {
                    //Sort tasks by due time, then by urgency, importance, and finally alphabetically by name
                    var sortedTasks = tasksOnThatDay
                        .OrderBy(t => t.getDueDate().TimeOfDay)
                        .ThenByDescending(t => t.getisUrgent())
                        .ThenByDescending(t => t.getisImportant())
                        .ThenBy(t => t.GetTaskName())
                        .ToList();

                    // add tasks with a letter label (a, b, c, etc.)       
                    char letter = 'a';
                    // For each task, we display the due time, name, project (if any), and tags (Urgent/Important)
                    foreach (var sortedTask in sortedTasks)
                    {
                        string time = sortedTask.getDueDate().ToString("hh:mm tt");
                        string tags = BuildTags(sortedTask);
                        string proj = ProjectLabel(sortedTask);

                        // add the task with its letter label, due time, name, project label, and tags

                        sb.AppendLine($"\t{letter}) {time} {sortedTask.GetTaskName()}{proj}{tags}");
                        letter++;
                    }
                }

                // Then display projects due on that date, if any (we check the groupedProjects dictionary for easy lookup)
                if (groupedProjects.TryGetValue(date, out var projectsOnThatDay))
                {
                    sb.AppendLine("\t-- Projects due --");

                    foreach (var project in projectsOnThatDay
                        // We sort projects by due time, then by urgency, importance, and finally alphabetically by name
                        .OrderBy(p => p.getDueDate().TimeOfDay)
                        .ThenByDescending(p => p.getisUrgent())
                        .ThenByDescending(p => p.getisImportant())
                        .ThenBy(p => p.GetProjectName()))
                    {
                        string ptime = project.getDueDate().ToString("hh:mm tt");
                        string ptags = BuildTags(project);
                        sb.AppendLine($"\t   • {ptime} {project.GetProjectName()}{ptags}");
                    }
                }

                sb.AppendLine();
                dayCounter++;
            }

            return sb.ToString();
        }


        // Helper method to build the tags string for a task based on its urgency and importance
        private string BuildTags(UtilsTask task)
        {
            List<string> tags = new();
            if (task.getisUrgent()) tags.Add("Urgent");
            if (task.getisImportant()) tags.Add("Important");
            return tags.Count > 0 ? $" [{string.Join(", ", tags)}]" : "";
        }

        // Overloaded helper method to build the tags string for a project based on its urgency and importance
        private string BuildTags(UtilsProject project)
        {
            List<string> tags = new();
            if (project.getisUrgent()) tags.Add("Urgent");
            if (project.getisImportant()) tags.Add("Important");
            return tags.Count > 0 ? $" [{string.Join(", ", tags)}]" : "";
        }

        // Helper method to get the project label for a task, if it belongs to a project
        private string ProjectLabel(UtilsTask task)
        {
            int pid = task.getProjectId();
            if (pid == -1) return "";

            var p = projects.FirstOrDefault(x => x.GetProjectId() == pid);
            return p == null ? " [Project: Unknown]" : $" [Project: {p.GetProjectName()}]";
        }
    }
}