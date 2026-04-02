using Demo_PIG_Tool.Manager;
using System;
using System.IO;
using Demo_PIG_Tool.Utils;

namespace CalorieTracker
{
    public partial class CalorieTrackerControl : UserControl
    {
        public CalorieTrackerControl()
        {
            InitializeComponent();
            loadData();
            RefreshListView();
            SubToolManager.UpdateDocx();
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
            SubToolManager.UpdateDocx();
            ShowLogForm();
            viewLogsButton.BackColor = Color.FromArgb(0, 120, 215);
            logTodayButton.BackColor = Color.FromArgb(0, 20, 215);
        }


        //  List view 
        private void RefreshListView()
        {
            SubToolManager.UpdateDocx();
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
        private void ShowLogForm(string date = "", string weight = "", string calories = "")
        {
            dateTextBox.Text = string.IsNullOrEmpty(date) ? UtilsDate.GetDate() : date;
            weightTextBox.Text = weight;
            caloriesTextBox.Text = calories;
            listView.Visible = false;
            editPanel.Visible = true;
            weightTextBox.Focus();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            string date = dateTextBox.Text.Trim();
            if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _))
            {
                MessageBox.Show("Please enter a valid date in YYYY-MM-DD format.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            submitHealthData(date, weight, calories);
            MessageBox.Show("Data logged successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshListView();
            SubToolManager.UpdateDocx();
            listView.Visible = true;
            editPanel.Visible = false;
        }

        private void EditEntryButton_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an entry to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ListViewItem selected = listView.SelectedItems[0];
            string date = selected.Text;
            string weight = selected.SubItems[1].Text;
            string calories = selected.SubItems[2].Text;

            deleteEntry(date);
            ShowLogForm(date, weight, calories);
            logTodayButton.BackColor = Color.FromArgb(0, 20, 215);
            viewLogsButton.BackColor = Color.FromArgb(0, 120, 215);
            SubToolManager.UpdateDocx();
        }

        private void DeleteEntryButton_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an entry to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string date = listView.SelectedItems[0].Text;
            DialogResult confirm = MessageBox.Show($"Delete the entry for {date}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                deleteEntry(date);
                RefreshListView();
            }
            SubToolManager.UpdateDocx();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            listView.Visible = true;
            editPanel.Visible = false;
        }

        private void mainPanel_Paint(object sender, PaintEventArgs e) { }

        // HealthTool logic

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

        void deleteEntry(string date)
        {
            string path = GetProjectHealthLogPath();
            if (!File.Exists(path)) return;

            string[] lines = File.ReadAllLines(path);
            var remaining = new System.Collections.Generic.List<string>();

            foreach (string line in lines)
            {
                if (!line.StartsWith(date))
                {
                    remaining.Add(line);
                }
            }
            File.WriteAllLines(path, remaining);
        }
    }
}
