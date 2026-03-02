using ProjectAndTaskManagerFormAppDemo.Utils;
namespace ProjectAndTaskManagerFormAppDemo
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

        private void ViewEditButton_Click(object sender, EventArgs e)
        {
            RefreshListView();
            listView.Visible = true;
            editPanel.Visible = false;
            viewEditButton.BackColor = Color.FromArgb(0, 20, 215);
            createTaskButton.BackColor = Color.FromArgb(0, 120, 215);
            createProjectButton.BackColor = Color.FromArgb(0, 120, 215);
            editTipLabel.Visible = true;
        }

        private void CreateTaskButton_Click(object sender, EventArgs e)
        {
            ShowCreateTaskForm();
            viewEditButton.BackColor = Color.FromArgb(0, 120, 215);
            createTaskButton.BackColor = Color.FromArgb(0, 20, 215);
            createProjectButton.BackColor = Color.FromArgb(0, 120, 215);
            editTipLabel.Visible = false;
        }

        private void CreateProjectButton_Click(object sender, EventArgs e)
        {
            ShowCreateProjectForm();
            viewEditButton.BackColor = Color.FromArgb(0, 120, 215);
            createTaskButton.BackColor = Color.FromArgb(0, 120, 215);
            createProjectButton.BackColor = Color.FromArgb(0, 20, 215);
            editTipLabel.Visible = false;
        }

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

        private void ShowCreateTaskForm()
        {
            editPanel.Controls.Clear();
            editPanel.Visible = true;
            listView.Visible = false;

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
            };
            editPanel.Controls.Add(saveButton);

            Button cancelButton = new Button { Text = "Cancel", Location = new Point(310, yPos), Width = 100, Height = 35, BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            cancelButton.Click += (s, e) =>
            {
                listView.Visible = true;
                editPanel.Visible = false;
            };
            editPanel.Controls.Add(cancelButton);
        }

        private void ShowCreateProjectForm()
        {
            editPanel.Controls.Clear();
            editPanel.Visible = true;
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
            };
            editPanel.Controls.Add(saveButton);

            Button cancelButton = new Button { Text = "Cancel", Location = new Point(310, yPos), Width = 100, Height = 35, BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            cancelButton.Click += (s, e) =>
            {
                listView.Visible = true;
                editPanel.Visible = false;
            };
            editPanel.Controls.Add(cancelButton);
        }

        private void ShowEditTaskForm(int taskId)
        {
            UtilsTask taskToEdit = tasks.FirstOrDefault(t => t.GetTaskId() == taskId);
            if (taskToEdit == null) return;

            editPanel.Controls.Clear();
            editPanel.Visible = true;
            listView.Visible = false;

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
                }
            };
            editPanel.Controls.Add(deleteButton);

            Button cancelButton = new Button { Text = "Cancel", Location = new Point(420, yPos), Width = 100, Height = 35, BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            cancelButton.Click += (s, e) =>
            {
                listView.Visible = true;
                editPanel.Visible = false;
            };
            editPanel.Controls.Add(cancelButton);
        }

        private void ShowEditProjectForm(int projectId)
        {
            UtilsProject projectToEdit = projects.FirstOrDefault(p => p.GetProjectId() == projectId);
            if (projectToEdit == null) return;

            editPanel.Controls.Clear();
            editPanel.Visible = true;
            listView.Visible = false;

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
                }
            };
            editPanel.Controls.Add(deleteButton);

            Button cancelButton = new Button { Text = "Cancel", Location = new Point(420, yPos), Width = 100, Height = 35,BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat };
            cancelButton.Click += (s, e) =>
            {
                listView.Visible = true;
                editPanel.Visible = false;
            };
            editPanel.Controls.Add(cancelButton);
        }

        private string GetProjectsAndTasksLogPath()
        {
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "projectsAndTasksLogs.txt"));
        }

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
    }
}
