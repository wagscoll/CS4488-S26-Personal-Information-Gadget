using System;
using System.IO;
using Demo_PIG_Tool.Utils;

namespace Demo_PIG_Tool
{
    public partial class CalorieTrackerControl : UserControl
    {
        public CalorieTrackerControl()
        {
            InitializeComponent();
            loadData();
            RefreshListView();
        }

        //  Button nav 
        private void ViewLogsButton_Click(object sender, EventArgs e)
        {
            RefreshListView();
            listView.Visible = true;
            editPanel.Visible = false;
            viewLogsButton.BackColor = Color.FromArgb(0, 20, 215);
            logTodayButton.BackColor = Color.FromArgb(0, 120, 215);
        }

        private void LogTodayButton_Click(object sender, EventArgs e)
        {
            ShowLogForm();
            viewLogsButton.BackColor = Color.FromArgb(0, 120, 215);
            logTodayButton.BackColor = Color.FromArgb(0, 20, 215);
        }

        //  List view 
        private void RefreshListView()
        {
            listView.Items.Clear();

            string healthLogs = getHealthLogs();
            if (string.IsNullOrWhiteSpace(healthLogs)) return;

            foreach (var line in healthLogs.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = line.Split(" | ");
                if (parts.Length == 3)
                {
                    var item = new ListViewItem(parts[0]);
                    item.SubItems.Add(parts[1]);
                    item.SubItems.Add(parts[2]);
                    listView.Items.Add(item);
                }
            }
        }

        // Log form 
        private void ShowLogForm()
        {
            dateValueLabel.Text = UtilsDate.GetDate();
            weightTextBox.Clear();
            caloriesTextBox.Clear();
            listView.Visible = false;
            editPanel.Visible = true;
            weightTextBox.Focus();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (!float.TryParse(weightTextBox.Text, out float weight) || !inputValidation(weight))
            {
                MessageBox.Show("Please enter a valid weight (0 - 100,000).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!float.TryParse(caloriesTextBox.Text, out float calories) || !inputValidation(calories))
            {
                MessageBox.Show("Please enter a valid calorie amount (0 - 100,000).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            submitHealthData(UtilsDate.GetDate(), weight, calories);
            MessageBox.Show("Data logged successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshListView();
            listView.Visible = true;
            editPanel.Visible = false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            listView.Visible = true;
            editPanel.Visible = false;
        }

        private void mainPanel_Paint(object sender, PaintEventArgs e) { }

        // HealthTool logic (copied verbatim from HealthTool.cs) 

        private void loadData() { } // logs read on demand via getHealthLogs()

        string GetProjectHealthLogPath()
        {
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "logs", "healthlogs.txt"));
        }

        string getHealthLogs()
        {
            string path = GetProjectHealthLogPath();
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return "";
        }

        bool inputValidation(float value)
        {
            if (value < 0 || value > 100000)
                return false;
            if (float.IsNaN(value))
                return false;
            return true;
        }

        void submitHealthData(string date, float weight, float calories)
        {
            string path = GetProjectHealthLogPath();
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            using (StreamWriter sw = File.AppendText(path))
            {
                string healthData = string.Join(" | ", date, weight, calories);
                sw.WriteLine(healthData);
            }
        }
    }
}
