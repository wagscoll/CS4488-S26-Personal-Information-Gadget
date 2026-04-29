using Demo_PIG_Tool.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using Demo_PIG_Tool.Utils;

namespace CalorieTracker
{
    public partial class CalorieTrackerControl : UserControl
    {
        private string? _editingEntry;  // Tracks the original entry being edited (if any) to allow for
                                        // updating/deletion without overwriting contents

        private static readonly Color BtnSelected   = Color.FromArgb(0, 20, 215);
        private static readonly Color BtnUnselected = Color.FromArgb(0, 120, 215);

        



        
        // -- Navigation Button State Management ---------------------------------------------------------------
        private void SetNavButton(bool viewLogsActive)
        {
            if(viewLogsActive)
                logTodayButton.BackColor = BtnUnselected;
            else
                logTodayButton.BackColor = BtnSelected;

            if(viewLogsActive)
                viewLogsButton.BackColor = BtnSelected;
            else
                viewLogsButton.BackColor = BtnUnselected;
        }
        // -- /Navigation Button  -----------------------------------------------------------------------------






        // -- Helper Methods ----------------------------------------------------------------------------------
        private bool ValidateForm(out float weight, out float calories)
        {
            weight = 0; calories = 0;

            // YYYY-MM-DD format validation
            if (!InputValidation.IsValidDate(dateTextBox.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid date in YYYY-MM-DD format.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Weight and calorie validation - must be valid floats within a reasonable range (0 - 100,000)
            if (!float.TryParse(weightTextBox.Text, out weight) || !InputValidation.IsValidFloat(weight))
            {
                MessageBox.Show("Please enter a valid weight (0 - 100,000).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!float.TryParse(caloriesTextBox.Text, out calories) || !InputValidation.IsValidFloat(calories))
            {
                MessageBox.Show("Please enter a valid calorie amount (0 - 100,000).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool TryGetSelected(string action, out ListViewItem selected)
        {
            // Ensures an entry is selected in the list view before attempting to edit or delete
            if (listView.SelectedItems.Count == 0)
            {
                MessageBox.Show($"Please select an entry to {action}.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                selected = null!;
                return false;
            }
            selected = listView.SelectedItems[0];
            return true;
        }

        private void RefreshListView()
        {
            SubToolManager.UpdateDocx(); // Ensure we have the latest data before refreshing the view
            listView.Items.Clear();      // Clear existing entries to avoid duplicates

            string healthLogs = getHealthLogs(); 
            if (string.IsNullOrWhiteSpace(healthLogs)) return;

            foreach (var line in healthLogs.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = line.Split(" | "); // Delimeter used - format: date | weight | calories
                if (parts.Length == 3)
                {
                    var item = new ListViewItem(parts[0].Trim());
                    item.SubItems.Add(parts[1].Trim());
                    item.SubItems.Add(parts[2].Trim());
                    listView.Items.Add(item);
                }
            }
        }
        // -- /Helper Methods ----------------------------------------------------------------------------------






        // -- Initialization -----------------------------------------------------------------------------------
        public CalorieTrackerControl()
        {
            InitializeComponent();
            RefreshListView();
            SubToolManager.UpdateDocx();
        }

        private void mainPanel_Paint(object sender, PaintEventArgs e) { }

        private void ShowLogForm(string date = "", string weight = "", string calories = "")
        {
            dateTextBox.Text = string.IsNullOrEmpty(date) ? UtilsDate.GetDate() : date;
            weightTextBox.Text = weight;
            caloriesTextBox.Text = calories;
            listView.Visible = false;
            editPanel.Visible = true;
            weightTextBox.Focus();
        }
        // -- /Initialization ---------------------------------------------------------------------------------






        // -- Event Handlers - Button clicks ------------------------------------------------------------------

        private void ViewLogsButton_Click(object sender, EventArgs e)
        {
            RefreshListView();
            listView.Visible = true;
            editPanel.Visible = false;
            SetNavButton(viewLogsActive: true);
        }

        private void LogTodayButton_Click(object sender, EventArgs e)
        {
            SubToolManager.UpdateDocx();
            ShowLogForm(); // Opens the log form with today's date pre-filled
            SetNavButton(viewLogsActive: false);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateForm(out float weight, out float calories)) return;

            string date = dateTextBox.Text.Trim();

            if (_editingEntry != null)
            {
                DeleteEntry(_editingEntry);
                _editingEntry = null;
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
            if (!TryGetSelected("edit", out var selected)) return;

            string date = selected.Text;
            string weight = selected.SubItems[1].Text;
            string calories = selected.SubItems[2].Text;

            _editingEntry = string.Join(" | ", date, weight, calories);
            ShowLogForm(date, weight, calories);
            SetNavButton(viewLogsActive: false);
        }

        private void DeleteEntryButton_Click(object sender, EventArgs e)
        {
            if (!TryGetSelected("delete", out var selected)) return;

            string date = selected.Text;
            string entry = string.Join(" | ", date, selected.SubItems[1].Text, selected.SubItems[2].Text);

            DialogResult confirm = MessageBox.Show($"Delete the entry for {date}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                DeleteEntry(entry);
                RefreshListView();
                SubToolManager.UpdateDocx();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            _editingEntry = null;
            listView.Visible = true;
            editPanel.Visible = false;
        }
        // -- /Event Handlers - Button clicks ------------------------------------------------------------------
    





        // -- File I/O for health logs -------------------------------------------------------------------------
        string GetProjectHealthLogPath()
        {
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "logs", "healthlogs.txt"));
        }

        string getHealthLogs()
        {
            string path = GetProjectHealthLogPath();
            return File.Exists(path) ? File.ReadAllText(path) : "";
        }

        void submitHealthData(string date, float weight, float calories)
        {
            string path = GetProjectHealthLogPath();
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            using StreamWriter sw = File.AppendText(path);
            sw.WriteLine(string.Join(" | ", date, weight, calories));
        }

        void DeleteEntry(string entry)
        {
            string path = GetProjectHealthLogPath();
            if (!File.Exists(path)) return;

            string[] lines = File.ReadAllLines(path);
            bool removed = false;
            var remaining = new List<string>();

            foreach (string line in lines)
            {
                if (!removed && line.Trim() == entry.Trim())
                    removed = true;
                else
                    remaining.Add(line);
            }

            File.WriteAllLines(path, remaining);
        }
        // -- /File I/O for health logs --------------------------------------------------------------------
    }
}
